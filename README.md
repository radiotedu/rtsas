<p align="center">
  <img src="assets/branding/rtsas-logo.png" width="560" alt="RTSAS logo">
</p>

<h1 align="center">RTSAS — RadioTEDU Stream Application Suite</h1>

<p align="center">
  <b>High-Performance Icecast & SHOUTcast Live Stream Monitor, Security & Remote Control Suite</b>
</p>

<p align="center">
  <a href="LICENSE"><img alt="MIT License" src="https://img.shields.io/badge/license-MIT-blue.svg"></a>
  <img src="https://img.shields.io/badge/.NET-10.0%20%7C%208.0-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt=".NET 10.0 / 8.0" />
  <img src="https://img.shields.io/badge/Platform-Windows%20Forms-0078D7?style=flat-square&logo=windows&logoColor=white" alt="Windows Forms" />
  <img src="https://img.shields.io/badge/Security-DPAPI%20%2B%20PBKDF2-blueviolet?style=flat-square" alt="DPAPI + PBKDF2 Security" />
  <img src="https://img.shields.io/badge/Architecture-Host%20%2B%20Remote%20Client-orange?style=flat-square" alt="Host + Remote Client" />
  <img src="https://img.shields.io/badge/RAM%20Usage-%3C%2030MB-brightgreen?style=flat-square" alt="RAM Footprint" />
  <img src="https://img.shields.io/badge/CI%2FCD-GitHub%20Actions-2088FF?style=flat-square&logo=githubactions&logoColor=white" alt="CI/CD" />
</p>

RTSAS is the streaming monitoring, management, and automation suite for the **RadioTEDU** and **RTAI** ecosystem. It is engineered to monitor live and automated audio streams across **Icecast 2** and **SHOUTcast (v1/v2)** servers, keep station and track metadata synchronized, inspect connected listener IPs, and provide a **stateless LAN Remote Control companion client** with role-based access control (RBAC).

---

## 🏗️ System Architecture & Data Flow

```mermaid
graph TB
    subgraph StreamServers["📡 Streaming Media Servers"]
        IC["Icecast 2 Server<br/>/status-json.xsl & /admin/stats.xml"]
        SC1["SHOUTcast v1<br/>/7.html (CSV)"]
        SC2["SHOUTcast v2<br/>/stats?json=1"]
        ICY["Direct ICY Stream<br/>Icy-MetaData Header Probing"]
    end

    subgraph HostApp["🖥️ Host Monitor Application (RTSAS Host)"]
        Scanner["Multi-Protocol Probe Engine<br/>SocketsHttpHandler + Concurrency Throttle"]
        Cache["Live Status Cache &<br/>24h Snapshot Log Service"]
        Audit["Configuration Change<br/>Audit Trail Console"]
        Sec["Security Layer<br/>Windows DPAPI + PBKDF2 Hashing"]
        ApiServer["Embedded REST API Server<br/>HttpListener (:8080)"]
        HostUI["Main Dashboard (WinForms)<br/>Vector GDI+ Charts & IP Popups"]
    end

    subgraph RemoteClients["💻 LAN Remote Clients (Stateless)"]
        RemoteApp["Remote Control App<br/>Zero Local Storage"]
        SuperAdminUser["👑 SuperAdmin<br/>(Full Access + Channel Mgmt)"]
        OperatorUser["🛠️ Operator<br/>(Status + Logs + IPs)"]
        ViewerUser["👁️ Viewer<br/>(Status + Logs Only | IPs Hidden)"]
    end

    IC -->|HTTP JSON / XML| Scanner
    SC1 -->|HTTP HTML| Scanner
    SC2 -->|HTTP JSON| Scanner
    ICY -->|HTTP ICY Stream| Scanner

    Scanner --> Cache
    Cache --> HostUI
    Audit --> HostUI
    Sec --> Cache
    Cache --> ApiServer

    ApiServer -->|HTTP REST JSON Token Auth| RemoteApp
    RemoteApp --> SuperAdminUser
    RemoteApp --> OperatorUser
    RemoteApp --> ViewerUser
```

---

## ⚡ Key Highlights & Capabilities

- 🎯 **Lightweight & Blazing Fast**: Pure .NET Windows Forms with asynchronous non-blocking I/O (`SocketsHttpHandler`, `SemaphoreSlim`). Memory footprint **< 30 MB** with negligible CPU overhead.
- 📡 **Stateless Remote Control Companion App**: Manage and inspect streams from any computer across the local area network (LAN). Operates **statelessly in-memory** without caching or storing files on client machines.
- 👥 **Multi-User RBAC & Granular Permissions**: Define users with granular capability flags (`CanViewStatus`, `CanViewLogs`, `CanViewListenerIps`, `CanRefreshStreams`, `CanManageChannels`, `CanViewAuditLogs`). Restrict sensitive listener client IPs or channel modifications for normal viewers!
- 🔒 **Encrypted Password Security & PBKDF2 Hashing**: Passwords stored in `config.json` are encrypted at rest using **Windows DPAPI (`ProtectedData.Protect`)**. Master admin passwords are cryptographically salted and hashed using **PBKDF2 SHA-256 (100,000 iterations)**.
- 🛡️ **Per-Channel Security Verification**: Prompts for Master Admin authentication before editing or viewing credentials for each channel independently (no sticky bypasses).
- 👥 **Connected Listener Client IP Inspection**: Real-time popup displaying connected client IPs, location/network type, connected durations, and player user-agents (VLC, Chrome, Winamp, Foobar2000, AIMP).
- 📈 **24-Hour Live Analytics & Vector Charts**: Smooth anti-aliased GDI+ area charts for **Concurrent Listeners**, **Data Transfer & Bandwidth (MB/GB)**, and **Response Latency (Ping)** with crosshair hover tooltips and CSV export.
- 📋 **Configuration Audit Console**: Real-time audit trail tracking password alterations, mount changes, port updates, channel state toggles, and stream failure alerts.
- 📦 **Setup Wizard Installer**: Complete 4-step wizard that validates server inputs, configures initial mountpoints, and sets up desktop shortcuts.
- 🚀 **Automated GitHub Updates**: Built-in GitHub Releases checker with in-app changelog viewer and one-click updates.

---

## 📊 Protocol Compatibility Matrix

```mermaid
flowchart LR
    Start["Probe Target Stream"] --> CheckIC{"Icecast 2<br/>status-json.xsl?"}
    CheckIC -- Yes --> P1["Parse JSON Mountpoint Stats"]
    CheckIC -- No --> CheckSC2{"SHOUTcast v2<br/>stats?json=1?"}
    CheckSC2 -- Yes --> P2["Parse SHOUTcast JSON Stats"]
    CheckSC2 -- No --> CheckSC1{"SHOUTcast v1<br/>7.html?"}
    CheckSC1 -- Yes --> P3["Parse CSV Stream Data"]
    CheckSC1 -- No --> CheckXML{"Icecast Admin<br/>stats.xml?"}
    CheckXML -- Yes --> P4["Parse Admin XML Stats"]
    CheckXML -- No --> P5["Direct ICY Header Stream Probing"]
```

| Server Engine | Endpoint / Mechanism | Extracted Data |
| :--- | :--- | :--- |
| **Icecast 2 (JSON)** | `/status-json.xsl` | Listeners, Peak, Bitrate, Format, Title, Genre, Uptime |
| **Icecast 2 (Admin XML)** | `/admin/stats.xml` | Server metrics, connected listener IP list, user-agents |
| **SHOUTcast v2 (JSON)** | `/stats?json=1` | Listeners, Peak, Bitrate, Content-Type, Current Track |
| **SHOUTcast v1 (HTML/CSV)** | `/7.html` | Listeners, Server Status, Peak, Max, Bitrate, Song Title |
| **Direct ICY Probing** | `Icy-MetaData: 1` | `icy-br`, `icy-name`, `icy-genre`, `StreamTitle` chunk stream |

---

## 🚀 Installation & Getting Started

### Method 1: Windows Setup Installer Wizard (Recommended)
1. Download the latest **`IcecastStreamMonitor.Installer.exe`** from [GitHub Releases](https://github.com/radiotedu/rtsas/releases).
2. Run the installer wizard:
   - Select installation directory.
   - Enter your Server Host/IP, Admin Password, and Stream Password.
   - Click `+ Add Mount` to add your channels (e.g. `/live`, `/lofi`, `/rock`).
   - Select desktop shortcut preferences and click **Install**.

### Method 2: Build from Source (.NET 10 SDK)
```powershell
# Clone the repository
git clone https://github.com/radiotedu/rtsas.git
cd rtsas

# Run test suite (29 tests)
dotnet test IcecastStreamMonitor.slnx

# Run Host Application
dotnet run --project IcecastStreamMonitor/IcecastStreamMonitor.csproj

# Run Remote Client App
dotnet run --project IcecastStreamMonitor.RemoteControl/IcecastStreamMonitor.RemoteControl.csproj
```

---

## ⌨️ Keyboard Shortcuts

| Shortcut | Action |
| :--- | :--- |
| `Ctrl + N` | Add New Channel / Mountpoint |
| `Ctrl + E` / `Enter` | Edit Selected Channel (Prompts Admin Auth) |
| `Delete` | Remove Selected Channel |
| `Ctrl + D` | Duplicate Selected Channel |
| `Ctrl + T` / `Space` | Toggle Active / Disabled |
| `Ctrl + G` | Open 24-Hour Analytics & Charts Dialog |
| `Ctrl + L` | Open Connected Listener IPs Popup |
| `F5` | Refresh All Streams Immediately |
| `Ctrl + ,` | Open Global Settings & Tray Preferences |

---

## 📁 Repository Structure

```
rtsas/
├── assets/branding/                 # Official RadioTEDU, RTAI, and RTSAS Branding Logos
├── IcecastStreamMonitor/             # Core Host Monitor Application & REST API
│   ├── Controls/                     # GDI+ Vector Historical Chart Control
│   ├── Forms/                        # Main Dashboard, Channel Edit, Analytics, Listeners, Remote Users Dialogs
│   ├── Models/                       # AppSettings, StreamChannel, Listener, AuditLog, RemoteUser
│   └── Services/                     # Monitor, Probing, History, Audit, SecurityHelper, RemoteApiService
├── IcecastStreamMonitor.RemoteControl/# Zero-Storage Stateless LAN Companion Client App
│   ├── Controls/                     # Remote Vector Charts Control
│   ├── Forms/                        # Remote Login, Remote Dashboard, Analytics, Listeners Dialogs
│   ├── Models/                       # Remote Models & RBAC Permissions
│   └── Services/                     # RemoteApiClient (REST API Client)
├── IcecastStreamMonitor.Installer/   # Standalone Setup Installer Wizard
├── IcecastStreamMonitor.Tests/       # xUnit Automated Unit & Integration Tests (29 Passed)
├── .github/workflows/               # GitHub Actions CI/CD Release Pipeline
├── ARCHITECTURE.md                  # Comprehensive Technical Architecture & Design Document
├── CONTRIBUTING.md                  # Contribution Guidelines & Code Standards
├── SECURITY.md                      # Security Disclosure Policy & Cryptographic Specifications
├── LICENSE                          # MIT License
└── README.md                        # Project Documentation
```

---

## 🏷️ Branding

<p align="center">
  <img src="assets/branding/radiotedu.png" width="520" alt="RadioTEDU logo"><br><br>
  <img src="assets/branding/rtai.png" width="330" alt="RTAI logo">
</p>

RTSAS uses the RadioTEDU broadcast-signal mark to identify it as part of the same open-source radio and AI tooling family.

---

## 📄 License

Licensed under the [MIT License](LICENSE).
