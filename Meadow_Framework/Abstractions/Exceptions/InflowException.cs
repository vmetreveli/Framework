using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Meadow_Framework.Abstractions.Exceptions;

/// <summary>
///     Represents the base exception class for all custom exceptions in the Inflow framework.
///     Provides flexible constructors for various exception scenarios with support for error codes, titles, messages, and log levels.
/// </summary>
public class InflowException : Exception
{
    public InflowException(string title)
        : this("APP_ERROR", title, null, null, LogLevel.Warning)
    {
    }

    public InflowException(string title, LogLevel logLevel)
        : this("APP_ERROR", title, null, null, logLevel)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(string code, string title)
        : this(code, title, null, null, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(string code, string title, LogLevel logLevel)
        : this(code, title, null, null, logLevel)
    {
    }

    public InflowException(string code, string title, string message)
        : this(code, title, message, null, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(string code, string title, string message, LogLevel logLevel)
        : this(code, title, message, null, logLevel)
    {
    }

    public InflowException(string code, string title, string message, Exception innerException)
        : this(code, title, message, innerException, LogLevel.Warning)
    {
    }

    public InflowException(string? code, string? title, string? message, Exception innerException, LogLevel logLevel)
        : base(message ?? title ?? code, innerException)
    {
        Code = code;
        Title = title;
        LogLevel = logLevel;
        ResponseHeaders = new Dictionary<string, string>();
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code)
        : this(code.ToString(), null, null, null, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, LogLevel logLevel)
        : this(code.ToString(), null, null, null, logLevel)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, string title)
        : this(code.ToString(), title, null, null, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, string title, LogLevel logLevel)
        : this(code.ToString(), title, null, null, logLevel)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, string title, string message)
        : this(code.ToString(), title, message, null, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, string title, string message, LogLevel logLevel)
        : this(code.ToString(), title, message, null, logLevel)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, string title, string message, Exception innerException)
        : this(code.ToString(), title, message, innerException, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, string title, string message, Exception innerException, LogLevel logLevel)
        : this(code.ToString(), title, message, innerException, logLevel)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer)
        : this(code.ToString(), localizer[code.ToString()], null, null, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, LogLevel logLevel)
        : this(code.ToString(), localizer[code.ToString()], null, null, logLevel)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, string message)
        : this(code.ToString(), localizer[code.ToString()], message, null, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, string message, LogLevel logLevel)
        : this(code.ToString(), localizer[code.ToString()], message, null, logLevel)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, string message, Exception innerException)
        : this(code.ToString(), localizer[code.ToString()], message, innerException, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, string message, Exception innerException,
        LogLevel logLevel)
        : this(code.ToString(), localizer[code.ToString()], message, innerException, logLevel)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, object[] localizerArguments)
        : this(code.ToString(), localizer[code.ToString(), localizerArguments], null, null, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, object[] localizerArguments, LogLevel logLevel)
        : this(code.ToString(), localizer[code.ToString(), localizerArguments], null, null, logLevel)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message)
        : this(code.ToString(), localizer[code.ToString(), localizerArguments], message, null, LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        LogLevel logLevel)
        : this(code.ToString(), localizer[code.ToString(), localizerArguments], message, null, logLevel)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        Exception innerException)
        : this(code.ToString(), localizer[code.ToString(), localizerArguments], message, innerException,
            LogLevel.Warning)
    {
    }

    [ExcludeFromCodeCoverage]
    public InflowException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        Exception innerException, LogLevel logLevel)
        : this(code.ToString(), localizer[code.ToString(), localizerArguments], message, innerException, logLevel)
    {
    }

    /// <summary>
    ///     Gets or sets the error code associated with the exception.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///     Gets or sets the title or summary of the exception.
    /// </summary>
    [SuppressMessage("Security", "SensitiveDataAnalyzer:Property contains sensitive data",
        Justification = "Not needed")]
    public string Title { get; set; }

    /// <summary>
    ///     Gets or sets the log level for this exception.
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    ///     Gets or sets the custom response headers to be included in the HTTP response.
    /// </summary>
    public Dictionary<string, string> ResponseHeaders { get; set; }
}