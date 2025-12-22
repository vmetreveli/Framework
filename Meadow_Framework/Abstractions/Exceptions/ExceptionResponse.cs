using System.Net;

namespace Meadow_Framework.Abstractions.Exceptions;

/// <summary>
///     Represents a response for an exception, containing the response object and HTTP status code.
/// </summary>
/// <param name="Response">The response object containing exception details.</param>
/// <param name="StatusCode">The HTTP status code associated with the exception.</param>
public record ExceptionResponse(object Response, HttpStatusCode StatusCode);