using Meadow_Framework.Framework.Abstractions.Exceptions;
using Microsoft.Extensions.Logging;

namespace Meadow_Framework.Framework.Infrastructure.Exceptions;

/// <summary>
///     Represents an exception thrown when a requested object is not found in the system.
/// </summary>
public sealed class ObjectNotFoundException : InflowException
{
    public ObjectNotFoundException(string objectType, string? objectId)
        : base("OBJECT_NOT_FOUND", "Object not found.",
            $"{objectType}:{(objectId is not null ? $":{objectId}" : null)} not found", null, LogLevel.Warning)
    {
    }


    public ObjectNotFoundException(string objectType, string? objectId, LogLevel logLevel)
        : base("OBJECT_NOT_FOUND", "Object not found.",
            $"{objectType}:{(objectId is not null ? $":{objectId}" : null)} not found", null, logLevel)
    {
    }
}