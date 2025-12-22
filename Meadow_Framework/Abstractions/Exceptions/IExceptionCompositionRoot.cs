namespace Meadow_Framework.Abstractions.Exceptions;

/// <summary>
///     Defines the interface for exception composition root that maps exceptions to exception responses.
/// </summary>
public interface IExceptionCompositionRoot
{
    /// <summary>
    ///     Maps an exception to an <see cref="ExceptionResponse" />.
    /// </summary>
    /// <param name="exception">The exception to map.</param>
    /// <returns>An <see cref="ExceptionResponse" /> representing the exception.</returns>
    ExceptionResponse Map(Exception exception);
}