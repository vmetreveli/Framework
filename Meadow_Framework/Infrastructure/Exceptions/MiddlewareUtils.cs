using Microsoft.AspNetCore.Builder;

namespace Meadow_Framework.Infrastructure.Exceptions;

/// <summary>
/// Helper extensions for registering middleware components related to exception handling.
/// </summary>
public static class MiddlewareUtils
{
    /// <summary>
    /// Registers the exception handling middleware into the application's request pipeline.
    /// This extension wraps <see cref="ExceptionMiddleware"/> and allows callers to provide
    /// an optional mapping from exception <see cref="Type"/> to HTTP status code that the
    /// middleware should use when writing responses for specific exception types.
    /// </summary>
    /// <param name="app">The application builder used to configure the HTTP request pipeline.</param>
    /// <param name="statusCodes">An optional dictionary that maps exception types to HTTP status codes.
    /// If <c>null</c> a new empty dictionary will be used and the middleware will rely on its defaults.</param>
    /// <returns>The same <see cref="IApplicationBuilder"/> instance so calls can be chained.</returns>
    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder app,
        Dictionary<Type, int> statusCodes = null)
    {
        app.UseMiddleware<ExceptionMiddleware>(statusCodes ?? new Dictionary<Type, int>());

        return app;
    }
}