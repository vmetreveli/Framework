using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Framework.Abstractions.Exceptions;

public class AppValidationException:InflowException
{
    public AppValidationException(string title) : base(title)
    {
    }

    public AppValidationException(string title, LogLevel logLevel) : base(title, logLevel)
    {
    }

    public AppValidationException(string code, string title) : base(code, title)
    {
    }

    public AppValidationException(string code, string title, LogLevel logLevel) : base(code, title, logLevel)
    {
    }

    public AppValidationException(string code, string title, string message) : base(code, title, message)
    {
    }

    public AppValidationException(string code, string title, string message, LogLevel logLevel) : base(code, title, message, logLevel)
    {
    }

    public AppValidationException(string code, string title, string message, Exception innerException) : base(code, title, message, innerException)
    {
    }

    public AppValidationException(string code, string title, string message, Exception innerException, LogLevel logLevel) : base(code, title, message, innerException, logLevel)
    {
    }

    public AppValidationException(Enum code) : base(code)
    {
    }

    public AppValidationException(Enum code, LogLevel logLevel) : base(code, logLevel)
    {
    }

    public AppValidationException(Enum code, string title) : base(code, title)
    {
    }

    public AppValidationException(Enum code, string title, LogLevel logLevel) : base(code, title, logLevel)
    {
    }

    public AppValidationException(Enum code, string title, string message) : base(code, title, message)
    {
    }

    public AppValidationException(Enum code, string title, string message, LogLevel logLevel) : base(code, title, message, logLevel)
    {
    }

    public AppValidationException(Enum code, string title, string message, Exception innerException) : base(code, title, message, innerException)
    {
    }

    public AppValidationException(Enum code, string title, string message, Exception innerException, LogLevel logLevel) : base(code, title, message, innerException, logLevel)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer) : base(code, localizer)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, LogLevel logLevel) : base(code, localizer, logLevel)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, string message) : base(code, localizer, message)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, string message, LogLevel logLevel) : base(code, localizer, message, logLevel)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, string message, Exception innerException) : base(code, localizer, message, innerException)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, string message, Exception innerException, LogLevel logLevel) : base(code, localizer, message, innerException, logLevel)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, object[] localizerArguments) : base(code, localizer, localizerArguments)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, object[] localizerArguments, LogLevel logLevel) : base(code, localizer, localizerArguments, logLevel)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message) : base(code, localizer, localizerArguments, message)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message, LogLevel logLevel) : base(code, localizer, localizerArguments, message, logLevel)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message, Exception innerException) : base(code, localizer, localizerArguments, message, innerException)
    {
    }

    public AppValidationException(Enum code, IStringLocalizer localizer, object[] localizerArguments, string message, Exception innerException, LogLevel logLevel) : base(code, localizer, localizerArguments, message, innerException, logLevel)
    {
    }
    
    public IDictionary<string,string[]> Failures { get; }

    private void AddFailure(List<ValidationFailure> failures)
    {
        IEnumerable<string> propertiesNames = failures
            .Select(x => x.PropertyName)
            .Distinct();

        foreach (var propertiesName in propertiesNames)
        {
            string[] propertiesFailures = failures
                .Where(failure => failure.PropertyName == propertiesName)
                .Select(failure => failure.ErrorMessage)
                .Distinct()
                .ToArray();

            Failures.Add(propertiesName, propertiesFailures);
        }
    }
}