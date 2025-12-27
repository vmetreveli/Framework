using System.Text.Json;
using System.Text.Json.Serialization;

namespace Meadow_Framework.Core.Infrastructure.Security;

/// <summary>
///
/// </summary>
public sealed class MaskedStringJsonConverter : JsonConverter<string>
{
    private readonly string _mask;

    /// <summary>
    ///
    /// </summary>
    /// <param name="mask"></param>
    public MaskedStringJsonConverter(string mask)
    {
        _mask = mask;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override string Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
        => reader.GetString()!;

    /// <summary>
    ///
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(
        Utf8JsonWriter writer,
        string value,
        JsonSerializerOptions options)
        => writer.WriteStringValue(_mask);
}
