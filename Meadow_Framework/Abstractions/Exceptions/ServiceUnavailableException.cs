using Microsoft.Extensions.Logging;

namespace Meadow_Framework.Abstractions.Exceptions;

/// <summary>
///     Represents an exception thrown when a service is temporarily unavailable or unreachable.
/// </summary>
public class ServiceUnavailableException : InflowException
{
    public ServiceUnavailableException(string title) : base("SERVICE_UNALIENABLE", title, null, null, LogLevel.Warning)
    {
    }

    public ServiceUnavailableException(string title, LogLevel logLevel) : base("SERVICE_UNALIENABLE", title, null, null,
        logLevel)
    {
    }
}