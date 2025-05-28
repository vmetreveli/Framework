using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Primitives;

namespace Framework.Abstractions;

public static class StringUtils
{
    /// <summary>
    ///     Checks if value is null, empty, or consists only of white-space characters
    ///     and returns null if any of the conditions are met, otherwise the text representation of the value.
    /// </summary>
    public static string OrNullIfNullOrWhiteSpace(this StringValues value)
    {
        return OrNullIfNullOrWhiteSpace(value.ToString());
    }

    /// <summary>
    ///     Checks if value is null, empty, or consists only of white-space characters
    ///     and returns null if any of the conditions are met, otherwise the value itself.
    /// </summary>
    public static string OrNullIfNullOrWhiteSpace(this string value)
    {
        return !string.IsNullOrWhiteSpace(value) ? value : null;
    }

    /// <summary>
    ///     Checks if value is null, empty, or consists only of white-space characters
    ///     and returns value of "or" parameter if any of the conditions are met, otherwise the value itself.
    /// </summary>
    public static string Or(this string value, string or)
    {
        return !string.IsNullOrWhiteSpace(value) ? value : or;
    }

    /// <summary>
    ///     Converts the string (pascal case) to camel case, by converting the first character to lowercase.
    /// </summary>
    public static string ToCamelCase(this string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            var first = char.ToLowerInvariant(value[0]).ToString();

            if (value.Length > 1) return first + value[1..];

            return first;
        }

        return value;
    }

    /// <summary>
    ///     Converts the value (ignores case, removes underscores) to an enum value
    ///     and returns null if value is null, empty, or consists only of white-space characters, otherwise enum value.
    /// </summary>
    public static TEnum? ToEnum<TEnum>(this StringValues value)
        where TEnum : struct
    {
        return ToEnum<TEnum>(value.ToString());
    }

    /// <summary>
    ///     Converts the value (ignores case, removes underscores) to an enum
    ///     and returns null if value is null, empty, or consists only of white-space characters, otherwise enum value.
    /// </summary>
    public static TEnum? ToEnum<TEnum>(this string value)
        where TEnum : struct
    {
        if (string.IsNullOrWhiteSpace(value)) return null;

        if (Enum.TryParse(typeof(TEnum), value.Replace("_", string.Empty), true, out var enumValue))
            return (TEnum)enumValue;

        return null;
    }

    /// <summary>
    ///     Indicates whether a specified string is valid JSON.
    /// </summary>
    public static bool IsJson(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;

        value = value.Trim();

        if ((value.StartsWith('{') && value.EndsWith('}')) || (value.StartsWith('[') && value.EndsWith(']')))
            try
            {
                using var doc = JsonDocument.Parse(value);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }

        return false;
    }

    /// <summary>
    ///     Encrypt data using System.Security.Cryptography.SymmetricAlgorithm.IV and specified key (32 characters).
    /// </summary>
    public static string Encrypt(this string data, string key)
    {
        using var aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;

        using MemoryStream memoryStream = new();
        using var encryptor = aes.CreateEncryptor(Encoding.UTF8.GetBytes(key), aes.IV);
        using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
        using (StreamWriter streamWriter = new(cryptoStream))
        {
            streamWriter.Write(data);
        }

        var vector = aes.IV;
        var content = memoryStream.ToArray();
        var result = new byte[vector.Length + content.Length];

        Buffer.BlockCopy(vector, 0, result, 0, vector.Length);
        Buffer.BlockCopy(content, 0, result, vector.Length, content.Length);

        return Convert.ToBase64String(result);
    }

    /// <summary>
    ///     Encrypt data using SHA-256 and specified salt.
    /// </summary>
    public static string EncryptSha256(this string data, string salt)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(string.Concat(data, salt)));

        StringBuilder sb = new();

        for (var i = 0; i < bytes.Length; i++) sb.Append(bytes[i].ToString("x2"));

        return sb.ToString();
    }

    /// <summary>
    ///     Decrypt data using System.Security.Cryptography.SymmetricAlgorithm.IV and specified key (32 characters).
    /// </summary>
    public static string Decrypt(this string data, string key)
    {
        var cipher = Convert.FromBase64String(data);
        var vector = new byte[16];

        Buffer.BlockCopy(cipher, 0, vector, 0, vector.Length);

        using var aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;

        using MemoryStream memoryStream = new(cipher, vector.Length, cipher.Length - vector.Length);
        using var decryptor = aes.CreateDecryptor(Encoding.UTF8.GetBytes(key), vector);
        using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
        using StreamReader streamReader = new(cryptoStream);

        return streamReader.ReadToEnd();
    }

    /// <summary>
    ///     Encrypt data deterministicly using ECB mode, specified key and hmac for data integrity.
    /// </summary>
    public static string EncryptDeterministic(this string data, string key, string hmacKey)
    {
        var result = Encrypt(data, key, hmacKey);
        return Convert.ToBase64String(result);
    }

    /// <summary>
    ///     Encrypt data deterministicly using ECB mode, specified key and hmac for data integrity. Gets lowercase hash
    ///     charat=cters only.
    /// </summary>
    /// <returns> encrypted string in lower case characters </returns>
    public static string EncryptLowercaseDeterministic(this string data, string key, string hmacKey)
    {
        var result = Encrypt(data, key, hmacKey);
        return ToHexString(result);
    }

    /// <summary>
    ///     Dencrypt data deterministicly using ECB mode, specified key and hmac for data integrity.
    /// </summary>
    public static string DecryptDeterministic(this string data, string key, string hmacKey)
    {
        var cipherBytes = Convert.FromBase64String(data);
        return Decrypt(key, hmacKey, cipherBytes);
    }

    /// <summary>
    ///     Dencrypt data deterministicly using ECB mode, specified key and hmac for data integrity. operates on lowercase hash
    ///     value.
    /// </summary>
    /// <returns> decrypts lowercased hash and return actual value. </returns>
    public static string DecryptLowercaseDeterministic(this string hexData, string key, string hmacKey)
    {
        var dataWithHmac = FromHexString(hexData);
        return Decrypt(key, hmacKey, dataWithHmac);
    }

    private static byte[] Encrypt(string data, string key, string hmacKey)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Mode = CipherMode.ECB; // Use ECB mode for determinism
            aes.Padding = PaddingMode.PKCS7;

            var encryptor = aes.CreateEncryptor(aes.Key, null);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(data);
                    }
                }

                var ciphertext = ms.ToArray();
                var hmac = ComputeHmac(ciphertext, hmacKey);

                var result = new byte[ciphertext.Length + hmac.Length];
                Array.Copy(ciphertext, 0, result, 0, ciphertext.Length);
                Array.Copy(hmac, 0, result, ciphertext.Length, hmac.Length);
                return result;
            }
        }
    }

    private static string Decrypt(string key, string hmacKey, byte[] dataWithHmac)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Mode = CipherMode.ECB; // Use ECB mode for determinism
            aes.Padding = PaddingMode.PKCS7;

            // Separate ciphertext and HMAC
            var ciphertext = new byte[dataWithHmac.Length - 32];
            var hmac = new byte[32];

            Array.Copy(dataWithHmac, 0, ciphertext, 0, ciphertext.Length);
            Array.Copy(dataWithHmac, ciphertext.Length, hmac, 0, hmac.Length);

            // Verify HMAC before decryption
            var computedHmac = ComputeHmac(ciphertext, hmacKey);
            if (!CompareArrays(computedHmac, hmac)) throw new CryptographicException("Invalid HMAC.");

            var decryptor = aes.CreateDecryptor(aes.Key, null);

            using (var ms = new MemoryStream(ciphertext))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }

    private static byte[] ComputeHmac(byte[] data, string hmacKey)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(hmacKey)))
        {
            return hmac.ComputeHash(data);
        }
    }

    private static bool CompareArrays(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;

        for (var i = 0; i < a.Length; i++)
            if (a[i] != b[i])
                return false;

        return true;
    }

    private static string ToHexString(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }

    private static byte[] FromHexString(string hex)
    {
        var bytes = new byte[hex.Length / 2];
        for (var i = 0; i < bytes.Length; i++) bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        return bytes;
    }
}