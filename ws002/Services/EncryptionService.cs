using System.Security.Cryptography;
using System.Text;

namespace ws002.Services;

/// <summary>
/// AES-256 encryption service for message encryption
/// </summary>
public class EncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;
    private readonly bool _enabled;

    /// <summary>
    /// Gets whether encryption is enabled
    /// </summary>
    public bool IsEnabled => _enabled;

    /// <summary>
    /// Creates a new EncryptionService instance
    /// </summary>
    /// <param name="key">Base64 encoded 32-byte AES key</param>
    /// <param name="iv">Base64 encoded 16-byte IV (optional, will be generated if null)</param>
    public EncryptionService(string? key, string? iv = null)
    {
        if (string.IsNullOrEmpty(key))
        {
            _enabled = false;
            _key = Array.Empty<byte>();
            _iv = Array.Empty<byte>();
            return;
        }

        _enabled = true;
        _key = Convert.FromBase64String(key);

        if (_key.Length != 32)
        {
            throw new ArgumentException("AES-256 key must be 32 bytes (256 bits)", nameof(key));
        }

        if (string.IsNullOrEmpty(iv))
        {
            _iv = new byte[16];
            RandomNumberGenerator.Fill(_iv);
        }
        else
        {
            _iv = Convert.FromBase64String(iv);
            if (_iv.Length != 16)
            {
                throw new ArgumentException("AES IV must be 16 bytes", nameof(iv));
            }
        }
    }

    /// <summary>
    /// Encrypts plaintext using AES-256-CBC
    /// </summary>
    /// <param name="plainText">Text to encrypt</param>
    /// <returns>Base64 encoded ciphertext (IV + encrypted data)</returns>
    public string Encrypt(string plainText)
    {
        if (!_enabled)
            return plainText;

        if (string.IsNullOrEmpty(plainText))
            return plainText;

        try
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Prepend IV to encrypted data
            var result = new byte[_iv.Length + encryptedBytes.Length];
            Buffer.BlockCopy(_iv, 0, result, 0, _iv.Length);
            Buffer.BlockCopy(encryptedBytes, 0, result, _iv.Length, encryptedBytes.Length);

            return Convert.ToBase64String(result);
        }
        catch (Exception)
        {
            // If encryption fails, return original text (fail open)
            return plainText;
        }
    }

    /// <summary>
    /// Decrypts ciphertext using AES-256-CBC
    /// </summary>
    /// <param name="cipherText">Base64 encoded ciphertext</param>
    /// <returns>Decrypted plaintext</returns>
    public string Decrypt(string cipherText)
    {
        if (!_enabled)
            return cipherText;

        if (string.IsNullOrEmpty(cipherText))
            return cipherText;

        try
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            // Extract IV from first 16 bytes
            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - 16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, 16);
            Buffer.BlockCopy(fullCipher, 16, cipher, 0, cipher.Length);

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
        catch (Exception)
        {
            // If decryption fails, return original text (fail open)
            return cipherText;
        }
    }

    /// <summary>
    /// Generates a new AES-256 key
    /// </summary>
    /// <returns>Base64 encoded 32-byte key</returns>
    public static string GenerateKey()
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        return Convert.ToBase64String(aes.Key);
    }

    /// <summary>
    /// Generates a new AES IV
    /// </summary>
    /// <returns>Base64 encoded 16-byte IV</returns>
    public static string GenerateIV()
    {
        using var aes = Aes.Create();
        aes.GenerateIV();
        return Convert.ToBase64String(aes.IV);
    }
}
