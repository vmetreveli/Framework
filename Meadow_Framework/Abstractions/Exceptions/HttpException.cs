using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Meadow_Framework.Abstractions.Exceptions;

/// <summary>
///     Represents an HTTP-related exception in the application.
///     Extends <see cref="InflowException" /> with HTTP-specific functionality.
/// </summary>
public class HttpException : InflowException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified title.
    /// </summary>
    /// <param name="title">The title of the exception.</param>
    public HttpException(string title) : base(title)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified title and log level.
    /// </summary>
    /// <param name="title">The title of the exception.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(string title, LogLevel logLevel) : base(title, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code and title.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    public HttpException(string code, string title) : base(code, title)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, title, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(string code, string title, LogLevel logLevel) : base(code, title, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, title, and message.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    /// <param name="message">The error message.</param>
    public HttpException(string code, string title, string message) : base(code, title, message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, title, message, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    /// <param name="message">The error message.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(string code, string title, string message, LogLevel logLevel) : base(code, title, message,
        logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, title, message, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HttpException(string code, string title, string message, Exception innerException) : base(code, title,
        message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, title, message, a reference to the inner exception that is the cause of this exception, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(string code, string title, string message, Exception innerException, LogLevel logLevel) : base(
        code, title, message, innerException, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code.
    /// </summary>
    /// <param name="code">The error code.</param>
    public HttpException(Enum code) : base(code)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(Enum code, LogLevel logLevel) : base(code, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code and title.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    public HttpException(Enum code, string title) : base(code, title)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, title, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(Enum code, string title, LogLevel logLevel) : base(code, title, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, title, and message.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    /// <param name="message">The error message.</param>
    public HttpException(Enum code, string title, string message) : base(code, title, message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, title, message, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    /// <param name="message">The error message.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(Enum code, string title, string message, LogLevel logLevel) : base(code, title, message,
        logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, title, message, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HttpException(Enum code, string title, string message, Exception innerException) : base(code, title, message,
        innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, title, message, a reference to the inner exception that is the cause of this exception, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="title">The title of the exception.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(Enum code, string title, string message, Exception innerException, LogLevel logLevel) : base(
        code, title, message, innerException, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code and a localizer.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    public HttpException(Enum code, IStringLocalizer localizer) : base(code, localizer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(Enum code, IStringLocalizer localizer, LogLevel logLevel) : base(code, localizer, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, and a message.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="message">The error message.</param>
    public HttpException(Enum code, IStringLocalizer localizer, string message) : base(code, localizer, message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, a message, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="message">The error message.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(Enum code, IStringLocalizer localizer, string message, LogLevel logLevel) : base(code,
        localizer, message, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, a message, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HttpException(Enum code, IStringLocalizer localizer, string message, Exception innerException) : base(code,
        localizer, message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, a message, a reference to the inner exception that is the cause of this exception, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(Enum code, IStringLocalizer localizer, string message, Exception innerException,
        LogLevel logLevel) : base(code, localizer, message, innerException, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, and localizer arguments.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="localizerArguments">The arguments for the localizer.</param>
    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments) : base(code, localizer,
        localizerArguments)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, localizer arguments, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="localizerArguments">The arguments for the localizer.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments, LogLevel logLevel) : base(
        code, localizer, localizerArguments, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, localizer arguments, and a message.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="localizerArguments">The arguments for the localizer.</param>
    /// <param name="message">The error message.</param>
    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message) : base(
        code, localizer, localizerArguments, message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, localizer arguments, a message, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="localizerArguments">The arguments for the localizer.</param>
    /// <param name="message">The error message.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        LogLevel logLevel) : base(code, localizer, localizerArguments, message, logLevel)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, localizer arguments, a message, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="localizerArguments">The arguments for the localizer.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        Exception innerException) : base(code, localizer, localizerArguments, message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpException"/> class with a specified error code, a localizer, localizer arguments, a message, a reference to the inner exception that is the cause of this exception, and log level.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="localizer">The string localizer.</param>
    /// <param name="localizerArguments">The arguments for the localizer.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    /// <param name="logLevel">The log level of the exception.</param>
    public HttpException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message,
        Exception innerException, LogLevel logLevel) : base(code, localizer, localizerArguments, message,
        innerException, logLevel)
    {
    }

    /// <summary>
    ///     Gets or sets the endpoint URL that caused the exception.
    /// </summary>
    public string Endpoint { get; set; }
}