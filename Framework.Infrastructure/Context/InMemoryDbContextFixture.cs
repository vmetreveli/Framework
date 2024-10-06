namespace Framework.Infrastructure.Context;

public class InMemoryDbContextFixture : IDisposable
{
    public InMemoryDbContextFixture()
    {
        var options = new DbContextOptionsBuilder<BaseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        Context = new BaseDbContext(options);
        Context.Database.EnsureCreated();
    }

    public BaseDbContext Context { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing) Context.Dispose();
    }
}