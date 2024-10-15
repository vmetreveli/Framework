using Microsoft.Extensions.Logging;

namespace Framework.Abstractions.Exceptions;

public class ServiceUnavailableException : InflowException
{
    public ServiceUnavailableException(string title) : base("SERVICE_UNVALIBLE", title, null, null, LogLevel.Warning)
    {
    }

    public ServiceUnavailableException(string title, LogLevel logLevel) : base("SERVICE_UNVALIBLE", title, null, null,
        logLevel)
    {
    }
}