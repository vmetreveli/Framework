using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Framework.Abstractions.Exceptions;

public class AppValidationException : InflowException
{
    public AppValidationException()
        : base("VALIDATION_ERROR", "Validation error(s) occurred", null, null, LogLevel.Warning)
    {
        Failures = new Dictionary<string, string[]>();
    }

    [ExcludeFromCodeCoverage]
    public AppValidationException(LogLevel logLevel)
        : base("VALIDATION_ERROR", "Validation error(s) occurred", null, null, logLevel)
    {
        Failures = new Dictionary<string, string[]>();
    }

    [ExcludeFromCodeCoverage]
    public AppValidationException(string message)
        : base("VALIDATION_ERROR", "Validation error(s) occurred", message, null, LogLevel.Warning)
    {
        Failures = new Dictionary<string, string[]>();
    }

    public AppValidationException(string message, LogLevel logLevel)
        : base("VALIDATION_ERROR", "Validation error(s) occurred", message, null, logLevel)
    {
        Failures = new Dictionary<string, string[]>();
    }

    public AppValidationException(string propertyName, string errorMessage)
        : this(errorMessage, LogLevel.Warning)
    {
        Failures.Add(propertyName, [errorMessage]);
    }

    [ExcludeFromCodeCoverage]
    public AppValidationException(string propertyName, string errorMessage, LogLevel logLevel)
        : this(errorMessage, logLevel)
    {
        Failures.Add(propertyName, [errorMessage]);
    }

    [ExcludeFromCodeCoverage]
    public AppValidationException(List<ValidationFailure> failures)
        : this(string.Join(' ', failures.Select(i => i.ErrorMessage)), LogLevel.Warning)
    {
        AddFailures(failures);
    }

    public AppValidationException(List<ValidationFailure> failures, LogLevel logLevel)
        : this(string.Join(' ', failures.Select(i => i.ErrorMessage)), logLevel)
    {
        AddFailures(failures);
    }

    public IDictionary<string, string[]> Failures { get; }

    private void AddFailures(List<ValidationFailure> failures)
    {
        var propertyNames = failures
            .Select(failure => failure.PropertyName)
            .Distinct();

        foreach (var propertyName in propertyNames)
        {
            var propertyFailures = failures
                .Where(failure => failure.PropertyName == propertyName)
                .Select(failure => failure.ErrorMessage)
                .Distinct()
                .ToArray();

            Failures.Add(propertyName, propertyFailures);
        }
    }
}