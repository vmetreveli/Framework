using Framework.Abstractions.Exceptions;

namespace Framework.Infrastructure.Exceptions;

/// <summary>
///     The entry point for handling exceptions by delegating them to registered exception mappers.
///     This class is responsible for composing different mappers to process exceptions.
/// </summary>
internal sealed class ExceptionCompositionRoot(IServiceProvider serviceProvider) : IExceptionCompositionRoot
{
    /// <summary>
    ///     Maps the provided exception to an <see cref="ExceptionResponse" /> using the available exception mappers.
    /// </summary>
    /// <param name="exception">The exception to map.</param>
    /// <returns>An <see cref="ExceptionResponse" /> with the appropriate error details and HTTP status code.</returns>
    public ExceptionResponse Map(Exception exception)
    {
        // Create a new scope to resolve services from the DI container.
        using var scope = serviceProvider.CreateScope();

        // Retrieve all registered exception mappers from the DI container.
        var mappers = scope.ServiceProvider.GetServices<IExceptionToResponseMapper>().ToArray();

        // Filter out the default ExceptionToResponseMapper to prioritize custom mappers.
        var nonDefaultMappers = mappers.Where(x => x is not ExceptionToResponseMapper);

        // Use the first custom mapper that successfully maps the exception (non-null result).
        var result = nonDefaultMappers
            .Select(x => x.Map(exception))
            .SingleOrDefault(x => x is not null);

        // If a custom mapper was able to map the exception, return the result.
        if (result is not null) return result;

        // Fallback to using the default ExceptionToResponseMapper if no custom mapper handled the exception.
        var defaultMapper = mappers.SingleOrDefault(x => x is ExceptionToResponseMapper);

        // Map the exception using the default mapper, if available.
        return defaultMapper?.Map(exception);
    }
}