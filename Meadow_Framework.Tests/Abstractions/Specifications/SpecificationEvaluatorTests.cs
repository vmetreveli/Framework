using FluentAssertions;
using Meadow_Framework.Abstractions.Primitives;
using Meadow_Framework.Abstractions.Specifications;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Meadow_Framework.Tests.Abstractions.Specifications;

public class SpecificationEvaluatorTests
{
    public class TestEntity : EntityBase<Guid>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<RelatedEntity> RelatedEntities { get; set; } = new();

        public TestEntity(Guid id, string name, int age) : base(id)
        {
            Name = name;
            Age = age;
        }
    }

    public class RelatedEntity : EntityBase<Guid>
    {
        public string Description { get; set; }

        public RelatedEntity(Guid id, string description) : base(id)
        {
            Description = description;
        }
    }

    public class TestDbContext : DbContext
    {
        public DbSet<TestEntity> TestEntities { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }
    }

    public class TestSpecification : Specification<TestEntity, Guid>
    {
        public TestSpecification(string name) : base(x => x.Name == name)
        {
        }
        
        public TestSpecification(int minAge) : base(x => x.Age >= minAge)
        {
        }

        public void AddIncludeRelated()
        {
            AddInclude(x => x.RelatedEntities);
        }

        public void AddOrderByName()
        {
            AddOrderBy(x => x.Name);
        }

        public void AddOrderByAgeDescending()
        {
            AddOrderByDescending(x => x.Age);
        }
    }

    private TestDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new TestDbContext(options);
        
        context.TestEntities.Add(new TestEntity(Guid.NewGuid(), "Alice", 30));
        context.TestEntities.Add(new TestEntity(Guid.NewGuid(), "Bob", 25));
        context.TestEntities.Add(new TestEntity(Guid.NewGuid(), "Charlie", 35));
        context.SaveChanges();

        return context;
    }

    [Fact]
    public void GetQuery_ShouldFilterByCriteria()
    {
        // Arrange
        using var context = GetDbContext();
        var spec = new TestSpecification("Alice");

        // Act
        var result = SpecificationEvaluator.GetQuery(context.TestEntities.AsQueryable(), spec).ToList();

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Alice");
    }

    [Fact]
    public void GetQuery_ShouldOrderBy()
    {
        // Arrange
        using var context = GetDbContext();
        var spec = new TestSpecification(0);
        spec.AddOrderByName();

        // Act
        var result = SpecificationEvaluator.GetQuery(context.TestEntities.AsQueryable(), spec).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Alice");
        result[1].Name.Should().Be("Bob");
        result[2].Name.Should().Be("Charlie");
    }

    [Fact]
    public void GetQuery_ShouldOrderByDescending()
    {
        // Arrange
        using var context = GetDbContext();
        var spec = new TestSpecification(0);
        spec.AddOrderByAgeDescending();

        // Act
        var result = SpecificationEvaluator.GetQuery(context.TestEntities.AsQueryable(), spec).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Charlie"); // 35
        result[1].Name.Should().Be("Alice");   // 30
        result[2].Name.Should().Be("Bob");     // 25
    }
}
