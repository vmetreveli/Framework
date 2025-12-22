using FluentAssertions;
using FluentValidation.Results;
using Meadow_Framework.Abstractions.Exceptions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Meadow_Framework.Tests.Abstractions.Exceptions;

public class AppValidationExceptionTests
{
    [Fact]
    public void Constructor_WithFailures_ShouldGroupFailuresByPropertyName()
    {
        // Arrange
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Property1", "Error1"),
            new ValidationFailure("Property1", "Error2"),
            new ValidationFailure("Property2", "Error3")
        };

        // Act
        var exception = new AppValidationException(failures, LogLevel.Error);

        // Assert
        exception.Failures.Should().HaveCount(2);
        exception.Failures["Property1"].Should().HaveCount(2);
        exception.Failures["Property1"].Should().Contain("Error1");
        exception.Failures["Property1"].Should().Contain("Error2");
        exception.Failures["Property2"].Should().HaveCount(1);
        exception.Failures["Property2"].Should().Contain("Error3");
    }

    [Fact]
    public void Constructor_WithPropertyNameAndErrorMessage_ShouldAddFailure()
    {
        // Arrange
        var propertyName = "Property1";
        var errorMessage = "Error1";

        // Act
        var exception = new AppValidationException(propertyName, errorMessage);

        // Assert
        exception.Failures.Should().HaveCount(1);
        exception.Failures[propertyName].Should().Contain(errorMessage);
    }
}
