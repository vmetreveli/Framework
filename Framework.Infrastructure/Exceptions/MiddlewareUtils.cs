using Microsoft.AspNetCore.Builder;

namespace Framework.Infrastructure.Exceptions;

public static class MiddlewareUtils
{
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app,
        Dictionary<Type, int> statusCodes = null)
    {
        app.UseMiddleware<ExceptionMiddleware>(statusCodes ?? new Dictionary<Type, int>());

        return app;
    }
}