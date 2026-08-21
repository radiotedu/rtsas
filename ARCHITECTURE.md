# 🏛️ Technical Architecture & System Design

This document details the internal design, concurrency model, data flow, cryptographic security, and network protocol pipelines of **Icecast & SHOUTcast Live Stream Monitor Suite**.

---

## 1. High-Level Component Diagram

```mermaid
graph TB
    subgraph CoreEngine["Core Engine & Background Services"]
        ConfigStorage["ConfigStorageService<br/>(JSON + DPAPI / PBKDF2)"]
        MonitorService["StreamMonitorService<br/>(SocketsHttpHandler + Concurrency Throttling)"]
        Parser["StreamParser<br/>(Icecast / SHOUTcast / ICY Header Parser)"]
        HistoryService["StreamHistoryService<br/>(24h Sliding Window Snapshot Logs)"]
        AuditService["AuditLogService<br/>(Real-Time Change Console & Event Bus)"]
        RemoteApi["RemoteApiService<br/>(Embedded HttpListener + RBAC Auth)"]
    end

    subgraph UserInterface["Host Windows Forms UI"]
        MainForm["MainForm (Double-Buffered Grid)"]
        ChartsControl["HistoricalChartControl (Vector GDI+)"]
        Dialogs["AdminAuth, ChannelEdit, Analytics, Listeners, RemoteUsers"]
    end

    subgraph RemoteClient["Remote Control Client App"]
        RemoteMain["RemoteMainForm (Stateless RAM Only)"]
        ApiClient["RemoteApiClient (HttpClient + Bearer Token)"]
    end

    ConfigStorage --> MainForm
    MonitorService --> Parser
    MonitorService --> MainForm
    MainForm --> HistoryService
    MainForm --> AuditService
    HistoryService --> ChartsControl
    
    MainForm --> RemoteApi
    RemoteApi --> ApiClient
    ApiClient --> RemoteMain
```

---

## 2. Multi-Protocol Probing Lifecycle

When the periodic timer triggers or a manual refresh is requested, `StreamMonitorService` initiates asynchronous HTTP probes with strict concurrency management:

```mermaid
sequenceDiagram
    autonumber
    participant UI as MainForm
    participant Mon as StreamMonitorService
    participant Sem as SemaphoreSlim (Max 6)
    participant Srv as Target Streaming Server

    UI->>Mon: RefreshStreamsAsync()
    loop For each configured channel
        Mon->>Sem: WaitAsync() (Rate Limiting)
        Note over Mon,Srv: Asynchronous Probe with SocketsHttpHandler
        Mon->>Srv: GET /status-json.xsl (Icecast 2 JSON)
        alt 200 OK & Valid JSON
            Srv-->>Mon: Return Mountpoint Metrics
        else Fallback 1: SHOUTcast v2
            Mon->>Srv: GET /stats?json=1
            alt 200 OK & JSON
                Srv-->>Mon: Return SHOUTcast Metrics
            else Fallback 2: SHOUTcast v1
                Mon->>Srv: GET /7.html
                alt 200 OK & CSV
                    Srv-->>Mon: Return 7.html Metrics
                else Fallback 3: Direct ICY Stream Probing
                    Mon->>Srv: GET /mount (Headers: Icy-MetaData: 1)
                    Srv-->>Mon: Stream Headers (icy-br, icy-name, StreamTitle)
                end
            end
        end
        Sem-->>Mon: Release()
        Mon->>UI: Update StreamStatus Cache
    end
    UI->>UI: Render Grid & Record 24h Snapshot
```

---

## 3. Security & Cryptography Architecture

```mermaid
flowchart TD
    subgraph Passwords["Password Ingestion & Storage"]
        RawPass["Plaintext Password<br/>(Entered in UI / Setup Wizard)"]
        DPAPI["Windows DPAPI<br/>ProtectedData.Protect(Scope.CurrentUser)"]
        PBKDF2["PBKDF2 SHA-256<br/>100,000 Iterations + 16-byte Salt"]
        ConfigFile["config.json on Disk"]
    end

    RawPass -->|Stream Credentials| DPAPI
    DPAPI -->|Ciphertext 'ENC:...'| ConfigFile

    RawPass -->|Master Admin Password| PBKDF2
    PBKDF2 -->|Hash 'PBKDF2:...:...'| ConfigFile

    subgraph Verification["Runtime Authentication Verification"]
        InputAdmin["User Enters Admin Password in Modal"]
        SaltExtract["Extract Stored Salt & Iteration Count"]
        ComputeHash["Compute PBKDF2 Hash"]
        Compare["Cryptographic Fixed-Time Comparison"]
        Unlock["Unlock Channel Edit / Settings / Delete"]
    end

    ConfigFile --> SaltExtract
    InputAdmin --> ComputeHash
    SaltExtract --> ComputeHash
    ComputeHash --> Compare
    Compare -- Match --> Unlock
```

---

## 4. Role-Based Access Control (RBAC) & API Specifications

The host application runs an embedded lightweight HTTP server on port `8080` (`RemoteApiService`). All endpoints except `/api/auth/login` require a valid session bearer token.

### API Endpoints:

| Endpoint | Method | Required Permission | Description |
| :--- | :--- | :--- | :--- |
| `/api/auth/login` | `POST` | *Public* | Authenticates username/password; returns JWT session token and user permission flags. |
| `/api/streams` | `GET` | `CanViewStatus` | Returns real-time status of all channels (online, listeners, bitrate, format, song title). |
| `/api/streams/{id}/history` | `GET` | `CanViewLogs` | Returns 24-hour snapshot logs and summary metrics for charting. |
| `/api/streams/{id}/listeners` | `GET` | `CanViewListenerIps` | Returns live connected client IP addresses and user-agents (**Restricted for normal viewers**). |
| `/api/streams/refresh` | `POST` | `CanRefreshStreams` | Triggers an immediate refresh cycle across all channels. |
| `/api/streams/manage` | `POST` | `CanManageChannels` | Adds or deletes a channel on the server (**SuperAdmin only**). |
| `/api/audit` | `GET` | `CanViewAuditLogs` | Returns configuration change audit logs. |

---

## 5. Historical Chart Vector Rendering Engine

The custom GDI+ control (`HistoricalChartControl`) renders high-DPI vector graphics directly into memory using hardware double-buffering:

- **Anti-Aliasing**: `SmoothingMode.AntiAlias` and `TextRenderingHint.ClearTypeGridFit`.
- **Area Fill**: Linear gradient brushes with alpha channel fading (`Color.FromArgb(60, Color)` to `Color.FromArgb(8, Color)`).
- **Crosshair Hover**: Calculates nearest geometric Euclidean distance on mouse hover and renders floating tooltip badges.
- **Auto-Scaling**: Dynamically computes 5-division Y-axis spans and time-spaced X-axis intervals.
