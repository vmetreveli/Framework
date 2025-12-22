using FluentAssertions;
using Meadow_Framework.Abstractions.Primitives;
using Xunit;

namespace Meadow_Framework.Tests.Abstractions.Primitives;

public class ValueObjectTests
{
    private class TestValueObject : ValueObject
    {
        public int Value1 { get; }
        public string Value2 { get; }

        public TestValueObject(int value1, string value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value1;
            yield return Value2;
        }
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenValuesAreEqual()
    {
        // Arrange
        var obj1 = new TestValueObject(1, "a");
        var obj2 = new TestValueObject(1, "a");

        // Act
        var result = obj1.Equals(obj2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenValuesAreNotEqual()
    {
        // Arrange
        var obj1 = new TestValueObject(1, "a");
        var obj2 = new TestValueObject(2, "b");

        // Act
        var result = obj1.Equals(obj2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenOtherIsNull()
    {
        // Arrange
        var obj1 = new TestValueObject(1, "a");

        // Act
        var result = obj1.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrue_WhenValuesAreEqual()
    {
        // Arrange
        var obj1 = new TestValueObject(1, "a");
        var obj2 = new TestValueObject(1, "a");

        // Act
        var result = obj1 == obj2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrue_WhenValuesAreNotEqual()
    {
        // Arrange
        var obj1 = new TestValueObject(1, "a");
        var obj2 = new TestValueObject(2, "b");

        // Act
        var result = obj1 != obj2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_ShouldBeEqual_WhenValuesAreEqual()
    {
        // Arrange
        var obj1 = new TestValueObject(1, "a");
        var obj2 = new TestValueObject(1, "a");

        // Act
        var hashCode1 = obj1.GetHashCode();
        var hashCode2 = obj2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }
}
