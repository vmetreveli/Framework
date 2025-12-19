using System.Net;

namespace Meadow_Framework.Framework.Abstractions.Exceptions;

public record ExceptionResponse(object Response, HttpStatusCode StatusCode);