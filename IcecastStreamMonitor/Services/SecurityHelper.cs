using System.Security.Cryptography;
using System.Text;

namespace IcecastStreamMonitor.Services;

public static class SecurityHelper
{
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("IcecastStreamMonitor-Secure-Entropy-v1");

    /// <summary>
    /// Encrypts a plaintext string using Windows Data Protection API (DPAPI) bound to the current user.
    /// </summary>
    public static string EncryptSecret(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return string.Empty;

        try
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = ProtectedData.Protect(plainBytes, Entropy, DataProtectionScope.CurrentUser);
            return "ENC:" + Convert.ToBase64String(cipherBytes);
        }
        catch
        {
            // Fallback AES or base64 if DPAPI fails
            return "ENC:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }
    }

    /// <summary>
    /// Decrypts a DPAPI ciphertext string.
    /// </summary>
    public static string DecryptSecret(string encryptedText)
    {
        if (string.IsNullOrEmpty(encryptedText))
            return string.Empty;

        if (!encryptedText.StartsWith("ENC:"))
        {
            // Unencrypted legacy fallback
            return encryptedText;
        }

        try
        {
            var base64 = encryptedText.Substring(4);
            var cipherBytes = Convert.FromBase64String(base64);
            var plainBytes = ProtectedData.Unprotect(cipherBytes, Entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(plainBytes);
        }
        catch
        {
            try
            {
                var base64 = encryptedText.Substring(4);
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// Computes salted PBKDF2 hash of a password for verification.
    /// </summary>
    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            return string.Empty;

        byte[] salt = new byte[16];
        RandomNumberGenerator.Fill(salt);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations: 100_000,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: 32);

        return $"PBKDF2:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    /// <summary>
    /// Verifies if a password matches a salted PBKDF2 hash or fallback plain string.
    /// </summary>
    public static bool VerifyPassword(string enteredPassword, string storedHashOrPlain)
    {
        if (string.IsNullOrEmpty(storedHashOrPlain))
            return string.IsNullOrEmpty(enteredPassword);

        if (storedHashOrPlain.StartsWith("PBKDF2:"))
        {
            var parts = storedHashOrPlain.Split(':');
            if (parts.Length != 3) return false;

            var salt = Convert.FromBase64String(parts[1]);
            var expectedHash = Convert.FromBase64String(parts[2]);

            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                enteredPassword,
                salt,
                iterations: 100_000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }

        // Direct comparison if not yet hashed
        return string.Equals(enteredPassword, storedHashOrPlain, StringComparison.Ordinal);
    }
}
