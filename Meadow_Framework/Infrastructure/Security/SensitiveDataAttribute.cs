using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Meadow_Framework.Infrastructure.Security;

/// <summary>
///
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SensitiveDataAttribute: Attribute
{
}

/// <summary>
///
/// </summary>
public class EncryptedStringConverter : JsonConverter<string>
{
    private readonly byte[] _key;

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    public EncryptedStringConverter(byte[] key)
    {
        _key = key;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrEmpty(value) ? value : Decrypt(value, _key);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        if (string.IsNullOrEmpty(value))
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(Encrypt(value, _key));
    }

    private static string Encrypt(string plainText, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, iv);

        using var ms = new MemoryStream();
        ms.Write(iv);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    private static string Decrypt(string cipherText, byte[] key)
    {
        try
        {
            var fullCipher = Convert.FromBase64String(cipherText);
            
            using var aes = Aes.Create();
            aes.Key = key;

            var iv = new byte[aes.BlockSize / 8];
            
            if (fullCipher.Length < iv.Length) 
                return cipherText;

            Array.Copy(fullCipher, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
        catch
        {
            return cipherText;
        }
    }
}