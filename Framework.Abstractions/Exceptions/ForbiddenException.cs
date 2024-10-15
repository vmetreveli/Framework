namespace Framework.Abstractions.Exceptions;

public class ForbiddenException(string userId)
    : InflowException("FORBIDDEN", $"User:{userId} is not allowed to perform this action");