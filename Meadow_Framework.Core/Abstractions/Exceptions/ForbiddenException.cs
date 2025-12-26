namespace Meadow_Framework.Core.Abstractions.Exceptions;

/// <summary>
///     Represents an exception thrown when a user is not allowed to perform a specific action.
/// </summary>
/// <param name="userId">The identifier of the user who attempted the forbidden action.</param>
public class ForbiddenException(string userId)
    : InflowException("FORBIDDEN", $"User:{userId} is not allowed to perform this action");