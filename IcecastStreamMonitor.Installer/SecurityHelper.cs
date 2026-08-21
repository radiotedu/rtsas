using System.Security.Cryptography;
using System.Text;

namespace IcecastStreamMonitor.Installer;

public static class SecurityHelper
{
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("IcecastStreamMonitor-Secure-Entropy-v1");

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
            return "ENC:" + Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }
    }

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
}
