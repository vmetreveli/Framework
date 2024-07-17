using System.Net;
using Framework.Abstractions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Framework.Infrastructure.Exceptions;

internal sealed class ErrorHandlerMiddleware(
    IExceptionCompositionRoot exceptionCompositionRoot,
    ILogger<ErrorHandlerMiddleware> logger)
    : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
            await HandleErrorAsync(context, exception);
        }
    }

    private async Task HandleErrorAsync(HttpContext context, Exception exception)
    {
        var errorResponse = exceptionCompositionRoot.Map(exception);
        context.Response.StatusCode = (int)(errorResponse?.StatusCode ?? HttpStatusCode.InternalServerError);
        var response = errorResponse?.Response;
        if (response is null) return;

        await context.Response.WriteAsJsonAsync(response);
    }
}