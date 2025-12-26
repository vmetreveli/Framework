using FluentAssertions;
using Meadow_Framework.Core.Abstractions;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Meadow_Framework.Tests.Abstractions;

public class StringUtilsTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("   ", null)]
    [InlineData("value", "value")]
    public void OrNullIfNullOrWhiteSpace_String_ShouldReturnExpectedResult(string? input, string? expected)
    {
        // Act
        var result = input.OrNullIfNullOrWhiteSpace();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("   ", null)]
    [InlineData("value", "value")]
    public void OrNullIfNullOrWhiteSpace_StringValues_ShouldReturnExpectedResult(string? input, string? expected)
    {
        // Arrange
        var stringValues = new StringValues(input);

        // Act
        var result = stringValues.OrNullIfNullOrWhiteSpace();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "default", "default")]
    [InlineData("", "default", "default")]
    [InlineData("   ", "default", "default")]
    [InlineData("value", "default", "value")]
    public void Or_ShouldReturnExpectedResult(string? input, string orValue, string expected)
    {
        // Act
        var result = input.Or(orValue);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("PascalCase", "pascalCase")]
    [InlineData("camelCase", "camelCase")]
    [InlineData("A", "a")]
    public void ToCamelCase_ShouldReturnExpectedResult(string? input, string? expected)
    {
        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be(expected);
    }

    public enum TestEnum
    {
        ValueOne,
        ValueTwo
    }

    [Theory]
    [InlineData("ValueOne", TestEnum.ValueOne)]
    [InlineData(" ", null)] // The implementation replaces underscores but Enum.TryParse is case insensitive. Wait, implementation replaces "_" with empty. So "value_one" becomes "valueone". Enum.TryParse("valueone", true) should work if "ValueOne" is the name.
    [InlineData("ValueTwo", TestEnum.ValueTwo)]
    [InlineData("Invalid", null)]
    [InlineData(null, null)]
    [InlineData("", null)]
    public void ToEnum_String_ShouldReturnExpectedResult(string? input, TestEnum? expected)
    {
        // Act
        var result = input.ToEnum<TestEnum>();

        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public void ToEnum_WithUnderscore_ShouldWork()
    {
        // Arrange
        string input = "Value_One";
        
        // Act
        var result = input.ToEnum<TestEnum>();
        
        // Assert
        result.Should().Be(TestEnum.ValueOne);
    }

    [Theory]
    [InlineData("{\"key\": \"value\"}", true)]
    [InlineData("[1, 2, 3]", true)]
    [InlineData("invalid json", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsJson_ShouldReturnExpectedResult(string? input, bool expected)
    {
        // Act
        var result = input.IsJson();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Encrypt_Decrypt_ShouldWork()
    {
        // Arrange
        string data = "secret data";
        string key = "12345678901234567890123456789012"; // 32 chars

        // Act
        string encrypted = data.Encrypt(key);
        string decrypted = encrypted.Decrypt(key);

        // Assert
        decrypted.Should().Be(data);
    }

    [Fact]
    public void EncryptDeterministic_DecryptDeterministic_ShouldWork()
    {
        // Arrange
        string data = "secret data";
        string key = "12345678901234567890123456789012"; // 32 chars
        string hmacKey = "hmac_key";

        // Act
        string encrypted = data.EncryptDeterministic(key, hmacKey);
        string decrypted = encrypted.DecryptDeterministic(key, hmacKey);

        // Assert
        decrypted.Should().Be(data);
    }
    
    [Fact]
    public void EncryptLowercaseDeterministic_DecryptLowercaseDeterministic_ShouldWork()
    {
        // Arrange
        string data = "secret data";
        string key = "12345678901234567890123456789012"; // 32 chars
        string hmacKey = "hmac_key";

        // Act
        string encrypted = data.EncryptLowercaseDeterministic(key, hmacKey);
        string decrypted = encrypted.DecryptLowercaseDeterministic(key, hmacKey);

        // Assert
        decrypted.Should().Be(data);
    }
}
