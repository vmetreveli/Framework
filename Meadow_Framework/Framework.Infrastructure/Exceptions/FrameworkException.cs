using Meadow_Framework.Framework.Abstractions.Exceptions;

namespace Meadow_Framework.Framework.Infrastructure.Exceptions;

public sealed class FrameworkException(string message) : InflowException(message)
{
}