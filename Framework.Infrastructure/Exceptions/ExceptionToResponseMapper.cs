using System.Collections.Concurrent;
using System.Net;
using Framework.Abstractions.Exceptions;
using Humanizer;

namespace Framework.Infrastructure.Exceptions;

/// <summary>
///     Maps exceptions to standardized HTTP responses. This class is responsible for
///     translating exceptions into responses with meaningful error codes and messages.
/// </summary>
internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
{
    // A thread-safe dictionary that stores error codes for different exception types.
    private static readonly ConcurrentDictionary<Type, string> Codes = new();

    /// <summary>
    ///     Maps the provided exception to a corresponding HTTP response with an error message.
    /// </summary>
    /// <param name="exception">The exception that occurred.</param>
    /// <returns>An <see cref="ExceptionResponse" /> containing error details and the HTTP status code.</returns>
    public ExceptionResponse Map(Exception exception)
    {
        // Match different exception types and generate appropriate responses.
        return exception switch
        {
            // Map custom InflowException to a 400 Bad Request response.
            InflowException ex => new ExceptionResponse(
                new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message)), HttpStatusCode.BadRequest),

            // Map all other exceptions to a 500 Internal Server Error response.
            Exception ex => new ExceptionResponse(
                new ErrorsResponse(new Error(GetErrorCode(ex), $"{ex.Message} {ex.InnerException}")),
                HttpStatusCode.InternalServerError)
        };
    }

    /// <summary>
    ///     Retrieves the error code for the given exception type, generating and caching it if necessary.
    ///     The error code is based on the exception type's name.
    /// </summary>
    /// <param name="exception">The exception for which to generate or retrieve an error code.</param>
    /// <returns>A string representing the error code.</returns>
    private static string GetErrorCode(object exception)
    {
        var type = exception.GetType();
        // Generate a humanized, underscored error code from the exception type name.
        return Codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
    }

    // Record to represent a single error with a code and message.
    private record Error(string Code, string Message);

    // Record to represent a response containing multiple errors.
    private record ErrorsResponse(params Error[] Errors);
}