using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Primitives;

namespace Meadow_Framework.Core.Abstractions;

public static class StringUtils
{
    /// <summary>
    /// Checks if value is null, empty, or consists only of white-space characters
    /// and returns null if any of the conditions are met, otherwise the text representation of the value.
    /// </summary>
    public static string OrNullIfNullOrWhiteSpace(this StringValues value)
    {
        return OrNullIfNullOrWhiteSpace(value.ToString());
    }

    /// <summary>
    /// Checks if value is null, empty, or consists only of white-space characters
    /// and returns null if any of the conditions are met, otherwise the value itself.
    /// </summary>
    public static string OrNullIfNullOrWhiteSpace(this string value)
    {
        return !string.IsNullOrWhiteSpace(value) ? value : null;
    }

    /// <summary>
    /// Checks if value is null, empty, or consists only of white-space characters
    /// and returns value of "or" parameter if any of the conditions are met, otherwise the value itself.
    /// </summary>
    public static string Or(this string value, string or)
    {
        return !string.IsNullOrWhiteSpace(value) ? value : or;
    }

    /// <summary>
    /// Converts the string (pascal case) to camel case, by converting the first character to lowercase.
    /// </summary>
    public static string ToCamelCase(this string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            string first = char.ToLowerInvariant(value[0]).ToString();

            if (value.Length > 1)
            {
                return first + value[1..];
            }

            return first;
        }

        return value;
    }

    /// <summary>
    /// Converts the value (ignores case, removes underscores) to an enum value
    /// and returns null if value is null, empty, or consists only of white-space characters, otherwise enum value.
    /// </summary>
    public static TEnum? ToEnum<TEnum>(this StringValues value)
        where TEnum : struct
    {
        return ToEnum<TEnum>(value.ToString());
    }

    /// <summary>
    /// Converts the value (ignores case, removes underscores) to an enum
    /// and returns null if value is null, empty, or consists only of white-space characters, otherwise enum value.
    /// </summary>
    public static TEnum? ToEnum<TEnum>(this string value)
        where TEnum : struct
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (Enum.TryParse(typeof(TEnum), value.Replace("_", string.Empty), true, out object enumValue))
        {
            return (TEnum)enumValue;
        }

        return null;
    }

    /// <summary>
    /// Indicates whether a specified string is valid JSON.
    /// </summary>
    public static bool IsJson(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        value = value.Trim();

        if ((value.StartsWith('{') && value.EndsWith('}')) || (value.StartsWith('[') && value.EndsWith(']')))
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(value);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        return false;
    }

    /// <summary>
    /// Encrypt data using System.Security.Cryptography.SymmetricAlgorithm.IV and specified key (32 characters).
    /// </summary>
    public static string Encrypt(this string data, string key)
    {
        using Aes aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;

        using MemoryStream memoryStream = new();
        using ICryptoTransform encryptor = aes.CreateEncryptor(Encoding.UTF8.GetBytes(key), aes.IV);
        using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
        using (StreamWriter streamWriter = new(cryptoStream))
        {
            streamWriter.Write(data);
        }

        byte[] vector = aes.IV;
        byte[] content = memoryStream.ToArray();
        byte[] result = new byte[vector.Length + content.Length];

        Buffer.BlockCopy(vector, 0, result, 0, vector.Length);
        Buffer.BlockCopy(content, 0, result, vector.Length, content.Length);

        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Encrypt data using SHA-256 and specified salt.
    /// </summary>
    public static string EncryptSha256(this string data, string salt)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(string.Concat(data, salt)));

        StringBuilder sb = new();

        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("x2"));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Decrypt data using System.Security.Cryptography.SymmetricAlgorithm.IV and specified key (32 characters).
    /// </summary>
    public static string Decrypt(this string data, string key)
    {
        byte[] cipher = Convert.FromBase64String(data);
        byte[] vector = new byte[16];

        Buffer.BlockCopy(cipher, 0, vector, 0, vector.Length);

        using Aes aes = Aes.Create();
        aes.Padding = PaddingMode.PKCS7;

        using MemoryStream memoryStream = new(cipher, vector.Length, cipher.Length - vector.Length);
        using ICryptoTransform decryptor = aes.CreateDecryptor(Encoding.UTF8.GetBytes(key), vector);
        using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
        using StreamReader streamReader = new(cryptoStream);

        return streamReader.ReadToEnd();
    }

    /// <summary>
    /// Encrypt data deterministicly using ECB mode, specified key and hmac for data integrity.
    /// </summary>
    public static string EncryptDeterministic(this string data, string key, string hmacKey)
    {
        byte[] result = Encrypt(data, key, hmacKey);
        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Encrypt data deterministicly using ECB mode, specified key and hmac for data integrity. Gets lowercase hash charat=cters only.
    /// </summary>
    /// <returns> encrypted string in lower case characters </returns>
    public static string EncryptLowercaseDeterministic(this string data, string key, string hmacKey)
    {
        byte[] result = Encrypt(data, key, hmacKey);
        return ToHexString(result);
    }

    /// <summary>
    /// Dencrypt data deterministicly using ECB mode, specified key and hmac for data integrity.
    /// </summary>
    public static string DecryptDeterministic(this string data, string key, string hmacKey)
    {
        byte[] cipherBytes = Convert.FromBase64String(data);
        return Decrypt(key, hmacKey, cipherBytes);
    }

    /// <summary>
    /// Dencrypt data deterministicly using ECB mode, specified key and hmac for data integrity. operates on lowercase hash value.
    /// </summary>
    /// <returns> decrypts lowercased hash and return actual value. </returns>
    public static string DecryptLowercaseDeterministic(this string hexData, string key, string hmacKey)
    {
        byte[] dataWithHmac = FromHexString(hexData);
        return Decrypt(key, hmacKey, dataWithHmac);
    }

    private static byte[] Encrypt(string data, string key, string hmacKey)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Mode = CipherMode.ECB; // Use ECB mode for determinism
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, null);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(data);
                    }
                }

                byte[] ciphertext = ms.ToArray();
                byte[] hmac = ComputeHmac(ciphertext, hmacKey);

                byte[] result = new byte[ciphertext.Length + hmac.Length];
                Array.Copy(ciphertext, 0, result, 0, ciphertext.Length);
                Array.Copy(hmac, 0, result, ciphertext.Length, hmac.Length);
                return result;
            }
        }
    }

    private static string Decrypt(string key, string hmacKey, byte[] dataWithHmac)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Mode = CipherMode.ECB; // Use ECB mode for determinism
            aes.Padding = PaddingMode.PKCS7;

            // Separate ciphertext and HMAC
            byte[] ciphertext = new byte[dataWithHmac.Length - 32];
            byte[] hmac = new byte[32];

            Array.Copy(dataWithHmac, 0, ciphertext, 0, ciphertext.Length);
            Array.Copy(dataWithHmac, ciphertext.Length, hmac, 0, hmac.Length);

            // Verify HMAC before decryption
            byte[] computedHmac = ComputeHmac(ciphertext, hmacKey);
            if (!CompareArrays(computedHmac, hmac))
            {
                throw new CryptographicException("Invalid HMAC.");
            }

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, null);

            using (MemoryStream ms = new MemoryStream(ciphertext))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }

    private static byte[] ComputeHmac(byte[] data, string hmacKey)
    {
        using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(hmacKey)))
        {
            return hmac.ComputeHash(data);
        }
    }

    private static bool CompareArrays(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                return false;
            }
        }

        return true;
    }

    private static string ToHexString(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }

    private static byte[] FromHexString(string hex)
    {
        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return bytes;
    }
}