using System.Net;

namespace Framework.Abstractions.Exceptions;

public record ExceptionResponse(object Response, HttpStatusCode StatusCode);