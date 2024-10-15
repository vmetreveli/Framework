using Framework.Abstractions.Exceptions;

namespace Framework.Infrastructure.Exceptions;

public sealed class FrameworkException(string message) : InflowException(message)
{
}