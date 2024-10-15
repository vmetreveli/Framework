using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Framework.Abstractions.Exceptions;

public class HttpException : InflowException
{
    public HttpException(string title) : base(title)
    {
    }

    public HttpException(string title, LogLevel logLevel) : base(title, logLevel)
    {
    }

    public HttpException(string code, string title) : base(code, title)
    {
    }

    public HttpException(string code, string title, LogLevel logLevel) : base(code, title, logLevel)
    {
    }

    public HttpException(string code, string title, string message) : base(code, title, message)
    {
    }

    public HttpException(string code, string title, string message, LogLevel logLevel) : base(code, title, message,
        logLevel)
    {
    }

    public HttpException(string code, string title, string message, Exception innerException) : base(code, title,
        message, innerException)
    {
    }

    public HttpException(string code, string title, string message, Exception innerException, LogLevel logLevel) : base(
        code, title, message, innerException, logLevel)
    {
    }

    public HttpException(Enum code) : base(code)
    {
    }

    public HttpException(Enum code, LogLevel logLevel) : base(code, logLevel)
    {
    }

    public HttpException(Enum code, string title) : base(code, title)
    {
    }

    public HttpException(Enum code, string title, LogLevel logLevel) : base(code, title, logLevel)
    {
    }

    public HttpException(Enum code, string title, string message) : base(code, title, message)
    {
    }

    public HttpException(Enum code, string title, string message, LogLevel logLevel) : base(code, title, message,
        logLevel)
    {
    }

    public HttpException(Enum code, string title, string message, Exception innerException) : base(code, title, message,
        innerException)
    {
    }

    public HttpException(Enum code, string title, string message, Exception innerException, LogLevel logLevel) : base(
        code, title, message, innerException, logLevel)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer) : base(code, localizer)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, LogLevel logLevel) : base(code, localizer, logLevel)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, string message) : base(code, localizer, message)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, string message, LogLevel logLevel) : base(code,
        localizer, message, logLevel)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, string message, Exception innerException) : base(code,
        localizer, message, innerException)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, string message, Exception innerException,
        LogLevel logLevel) : base(code, localizer, message, innerException, logLevel)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments) : base(code, localizer,
        localizerArguments)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments, LogLevel logLevel) : base(
        code, localizer, localizerArguments, logLevel)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message) : base(
        code, localizer, localizerArguments, message)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        LogLevel logLevel) : base(code, localizer, localizerArguments, message, logLevel)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        Exception innerException) : base(code, localizer, localizerArguments, message, innerException)
    {
    }

    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        Exception innerException, LogLevel logLevel) : base(code, localizer, localizerArguments, message,
        innerException, logLevel)
    {
    }

    public string Endpoint { get; set; }
}