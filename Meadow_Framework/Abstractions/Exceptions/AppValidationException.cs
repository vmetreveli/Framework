using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Meadow_Framework.Abstractions.Exceptions;

/// <summary>
///     Represents a validation exception that occurs when one or more validation rules are violated.
///     Contains a dictionary of validation failures grouped by property names.
/// </summary>
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

    /// <summary>
    ///     Gets the dictionary of validation failures, where the key is the property name
    ///     and the value is an array of error messages for that property.
    /// </summary>
    public IDictionary<string, string[]> Failures { get;  }

    /// <summary>
    ///     Adds validation failures to the Failures dictionary, grouping them by property name.
    /// </summary>
    /// <param name="failures">The list of validation failures to add.</param>
    private void AddFailures(List<ValidationFailure> failures)
    {
        IEnumerable<string> propertyNames = failures
            .Select(failure => failure.PropertyName)
            .Distinct();

        foreach (string propertyName in propertyNames)
        {
            string[] propertyFailures = failures
                .Where(failure => failure.PropertyName == propertyName)
                .Select(failure => failure.ErrorMessage)
                .Distinct()
                .ToArray();

            Failures.Add(propertyName, propertyFailures);
        }
    }
}