# 🤝 Contributing to Icecast Stream Monitor

Thank you for your interest in contributing to **Icecast & SHOUTcast Live Stream Monitor Suite**!

---

## 🛠️ Development Setup

### Prerequisites
- **.NET 10.0 SDK** (or .NET 8.0 SDK)
- **Windows 10 / 11** (Required for Windows Forms Designer)
- **Visual Studio 2022 / Rider / VS Code** with C# Dev Kit

### Cloning & Building
```powershell
git clone https://github.com/YOUR_GITHUB_USERNAME/IcecastStreamMonitor.git
cd IcecastStreamMonitor

# Restore dependencies
dotnet restore IcecastStreamMonitor.slnx

# Run automated test suite
dotnet test IcecastStreamMonitor.slnx

# Build Release packages
dotnet build IcecastStreamMonitor.slnx -c Release
```

---

## 📐 Coding Conventions & Guidelines

1. **Pure Standard Library**: Avoid introducing heavy external third-party dependencies unless strictly necessary.
2. **WinForms Designer Compatibility**: When adding custom properties to custom controls (like `HistoricalChartControl`), always decorate them with `[DesignerSerializationVisibility]` and `[DefaultValue]` to maintain compatibility with modern .NET Windows Forms designers.
3. **Double-Buffering**: Any new `DataGridView` or custom painted control must have double buffering enabled to prevent screen flicker.
4. **Security & Secrets**: Never store credentials or sensitive tokens in cleartext. Always route through `SecurityHelper.EncryptSecret` (DPAPI) or `SecurityHelper.HashPassword` (PBKDF2).
5. **Asynchronous I/O**: Use non-blocking `async/await` patterns for all network operations with proper timeout cancellation tokens.

---

## 🧪 Testing Protocol

- Every new feature or parser must include corresponding unit tests in `IcecastStreamMonitor.Tests`.
- Ensure all tests pass with zero failures before submitting a pull request:
  ```powershell
  dotnet test IcecastStreamMonitor.Tests/IcecastStreamMonitor.Tests.csproj
  ```

---

## 📬 Pull Request Process

1. Fork the repository and create a feature branch (`git checkout -b feature/awesome-feature`).
2. Commit your changes with clear, descriptive commit messages.
3. Push to your branch (`git push origin feature/awesome-feature`).
4. Open a Pull Request on GitHub with a summary of the changes, screenshots, and test results.
