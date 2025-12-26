using Meadow_Framework.Core.Abstractions.Exceptions;

namespace Meadow_Framework.Core.Infrastructure.Exceptions;

public sealed class FrameworkException(string message) : InflowException(message)
{
}