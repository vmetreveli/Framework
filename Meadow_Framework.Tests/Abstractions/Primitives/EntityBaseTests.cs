using FluentAssertions;
using Meadow_Framework.Abstractions.Primitives;
using Xunit;

namespace Meadow_Framework.Tests.Abstractions.Primitives;

public class EntityBaseTests
{
    private class TestEntity : EntityBase<Guid>
    {
        public TestEntity(Guid id) : base(id)
        {
        }
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenIdsAreEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenIdsAreNotEqual()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenOtherIsNull()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity1.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrue_WhenIdsAreEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var result = entity1 == entity2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrue_WhenIdsAreNotEqual()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act
        var result = entity1 != entity2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_ShouldBeEqual_WhenIdsAreEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }
}
