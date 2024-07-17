using System.Collections.Concurrent;
using System.Net;
using Framework.Abstractions.Exceptions;
using Humanizer;

namespace Framework.Infrastructure.Exceptions;

internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
{
    private static readonly ConcurrentDictionary<Type, string> Codes = new();

    public ExceptionResponse Map(Exception exception)
    {
        return exception switch
        {
            InflowException ex => new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message))
                , HttpStatusCode.BadRequest),
            //   _ => new ExceptionResponse(new ErrorsResponse(new Error("error", "There was an error.")),
            Exception ex => new ExceptionResponse(
                new ErrorsResponse(new Error(GetErrorCode(ex), $"{ex.Message} " +
                                                               $" {ex.InnerException}")),
                HttpStatusCode.InternalServerError)
        };
    }

    private static string GetErrorCode(object exception)
    {
        var type = exception.GetType();
        return Codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
    }

    private record Error(string Code, string Message);

    private record ErrorsResponse(params Error[] Errors);
}