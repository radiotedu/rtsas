# 🔒 Security Policy

## Supported Versions

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |

---

## 🛡️ Cryptographic Implementation Details

This suite employs standard cryptographic safeguards to ensure zero plaintext exposure of sensitive data:

1. **Secrets at Rest (DPAPI)**:
   - Stream passwords and credentials written to `config.json` are encrypted using Windows Data Protection API (`ProtectedData.Protect`) with `DataProtectionScope.CurrentUser` and custom cryptographic entropy.
   - Encrypted values are prefixed with `ENC:` in base64 format.

2. **Master Admin Password Verification (PBKDF2)**:
   - Administrative panel passwords are never stored in cleartext.
   - Verification uses **PBKDF2 SHA-256** with 100,000 iterations and a 16-byte cryptographically secure pseudorandom salt (`RandomNumberGenerator`).

3. **Remote Control Authentication (RBAC)**:
   - Sessions are protected via bearer tokens generated in memory with time-limited expiration.
   - Granular permissions are enforced on the server-side before executing any action or returning listener IP records.

---

## 🚨 Reporting a Vulnerability

If you discover a security vulnerability within Icecast Stream Monitor, please report it responsibly:

1. **Do NOT** file a public GitHub issue.
2. Email the maintainer directly or use GitHub's private security advisory feature under the **Security** tab of the repository.
3. Include a detailed description, reproduction steps, and potential impact.
4. You will receive an initial response within 48 hours, and patches will be released in an expedited security hotfix.
