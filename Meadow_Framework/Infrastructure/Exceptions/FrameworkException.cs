using Meadow_Framework.Abstractions.Exceptions;

namespace Meadow_Framework.Infrastructure.Exceptions;

public sealed class FrameworkException(string message) : InflowException(message)
{
}