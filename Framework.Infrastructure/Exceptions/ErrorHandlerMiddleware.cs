using System.Net;
using Framework.Abstractions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Framework.Infrastructure.Exceptions;

/// <summary>
///     Middleware to handle exceptions during the request pipeline.
///     It catches exceptions thrown in the application, logs them, and maps them to an appropriate HTTP response.
/// </summary>
internal sealed class ErrorHandlerMiddleware(
    IExceptionCompositionRoot exceptionCompositionRoot,
    ILogger<ErrorHandlerMiddleware> logger) : IMiddleware
{
    /// <summary>
    ///     Invokes the middleware to handle the HTTP request.
    ///     If an exception occurs, it is caught and handled by mapping to a corresponding error response.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" /> for the current request.</param>
    /// <param name="next">The next delegate/middleware in the request pipeline.</param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // Continue processing the request by invoking the next middleware in the pipeline.
            await next(context);
        }
        catch (Exception exception)
        {
            // Log the exception details for debugging and diagnostics.
            logger.LogError(exception, exception.Message);

            // Handle the exception by generating an appropriate error response.
            await HandleErrorAsync(context, exception);
        }
    }

    /// <summary>
    ///     Maps the caught exception to an appropriate error response and writes it to the HTTP response.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" /> for the current request.</param>
    /// <param name="exception">The exception that was thrown during request processing.</param>
    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        // Map the exception to an error response using the exception composition root.
        var errorResponse = exceptionCompositionRoot.Map(exception);

        // Set the HTTP status code for the response, defaulting to 500 Internal Server Error.
        context.Response.StatusCode = (int)(errorResponse?.StatusCode ?? HttpStatusCode.InternalServerError);

        // If there is a response body (error details), write it to the response in JSON format.
        var response = errorResponse?.Response;
        if (response is null) return;

        // Write the error response body to the HTTP response.
        await context.Response.WriteAsJsonAsync(response);
    }
}