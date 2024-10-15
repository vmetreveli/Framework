using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Framework.Abstractions.Exceptions;

public class OutdatedVersionException : InflowException
{
    public OutdatedVersionException(string title) : base(title)
    {
    }

    public OutdatedVersionException(string title, LogLevel logLevel) : base(title, logLevel)
    {
    }

    public OutdatedVersionException(string code, string title) : base(code, title)
    {
    }

    public OutdatedVersionException(string code, string title, LogLevel logLevel) : base(code, title, logLevel)
    {
    }

    public OutdatedVersionException(string code, string title, string message) : base(code, title, message)
    {
    }

    public OutdatedVersionException(string code, string title, string message, LogLevel logLevel) : base(code, title,
        message, logLevel)
    {
    }

    public OutdatedVersionException(string code, string title, string message, Exception innerException) : base(code,
        title, message, innerException)
    {
    }

    public OutdatedVersionException(string code, string title, string message, Exception innerException,
        LogLevel logLevel) : base(code, title, message, innerException, logLevel)
    {
    }

    public OutdatedVersionException(Enum code) : base(code)
    {
    }

    public OutdatedVersionException(Enum code, LogLevel logLevel) : base(code, logLevel)
    {
    }

    public OutdatedVersionException(Enum code, string title) : base(code, title)
    {
    }

    public OutdatedVersionException(Enum code, string title, LogLevel logLevel) : base(code, title, logLevel)
    {
    }

    public OutdatedVersionException(Enum code, string title, string message) : base(code, title, message)
    {
    }

    public OutdatedVersionException(Enum code, string title, string message, LogLevel logLevel) : base(code, title,
        message, logLevel)
    {
    }

    public OutdatedVersionException(Enum code, string title, string message, Exception innerException) : base(code,
        title, message, innerException)
    {
    }

    public OutdatedVersionException(Enum code, string title, string message, Exception innerException,
        LogLevel logLevel) : base(code, title, message, innerException, logLevel)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer) : base(code, localizer)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, LogLevel logLevel) : base(code, localizer,
        logLevel)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, string message) : base(code, localizer,
        message)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, string message, LogLevel logLevel) : base(
        code, localizer, message, logLevel)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, string message, Exception innerException) :
        base(code, localizer, message, innerException)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, string message, Exception innerException,
        LogLevel logLevel) : base(code, localizer, message, innerException, logLevel)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, object[] localizerArguments) : base(code,
        localizer, localizerArguments)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, object[] localizerArguments,
        LogLevel logLevel) : base(code, localizer, localizerArguments, logLevel)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message)
        : base(code, localizer, localizerArguments, message)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        LogLevel logLevel) : base(code, localizer, localizerArguments, message, logLevel)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        Exception innerException) : base(code, localizer, localizerArguments, message, innerException)
    {
    }

    public OutdatedVersionException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        Exception innerException, LogLevel logLevel) : base(code, localizer, localizerArguments, message,
        innerException, logLevel)
    {
    }
}