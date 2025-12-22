using FluentAssertions;
using Meadow_Framework.Abstractions.Primitives;
using Meadow_Framework.Abstractions.Specifications;
using Meadow_Framework.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Meadow_Framework.Tests.Infrastructure.Repository;

public class RepositoryBaseTests
{
    public class TestEntity : AggregateRoot<Guid>
    {
        public string Name { get; set; }

        public TestEntity(Guid id, string name) : base(id)
        {
            Name = name;
        }
    }

    public class TestDbContext : DbContext
    {
        public DbSet<TestEntity> TestEntities { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }
    }

    public class TestRepository : RepositoryBase<TestDbContext, TestEntity, Guid>
    {
        public TestRepository(TestDbContext context) : base(context)
        {
        }
    }
    
    public class TestSpecification : Specification<TestEntity, Guid>
    {
        public TestSpecification(string name) : base(x => x.Name == name)
        {
        }
    }

    private TestDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new TestDbContext(options);
        return context;
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        await using var context = GetDbContext();
        var repository = new TestRepository(context);
        var entity = new TestEntity(Guid.NewGuid(), "Test");

        // Act
        await repository.AddAsync(entity);
        await context.SaveChangesAsync();

        // Assert
        context.TestEntities.Should().Contain(entity);
    }

    /// <summary>
    ///
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity_WhenExists()
    {
        // Arrange
        await using var context = GetDbContext();
        var repository = new TestRepository(context);
        var id = Guid.NewGuid();
        var entity = new TestEntity(id, "Test");
        await context.TestEntities.AddAsync(entity);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(entity.Id);

        // Assert
        result.Should().Be(entity);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_WithSpecification_ShouldReturnEntity()
    {
        // Arrange
        await using var context = GetDbContext();
        var repository = new TestRepository(context);
        var entity1 = new TestEntity(Guid.NewGuid(), "Alice");
        var entity2 = new TestEntity(Guid.NewGuid(), "Bob");
        await context.TestEntities.AddRangeAsync(entity1, entity2);
        await context.SaveChangesAsync();

        var spec = new TestSpecification("Alice");

        // Act
        var result = await repository.FirstOrDefaultAsync(spec);

        // Assert
        result.Should().Be(entity1);
    }
    
    /// <summary>
    ///
    /// </summary>
    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenEntityExists()
    {
        // Arrange
        await using var context = GetDbContext();
        TestRepository repository = new TestRepository(context);

        TestEntity entity = new TestEntity(Guid.NewGuid(), "Test");
        await context.TestEntities.AddAsync(entity);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(entity.Id);

        // Assert
        result.Should().BeTrue();
    }
}
