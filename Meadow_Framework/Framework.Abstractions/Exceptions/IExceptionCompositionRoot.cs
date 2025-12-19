namespace Meadow_Framework.Framework.Abstractions.Exceptions;

public interface IExceptionCompositionRoot
{
    ExceptionResponse Map(Exception exception);
}