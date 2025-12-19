using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Framework.Infrastructure.Tests.Queries;

public class QueryDispatcherTests
{
    private class TestQuery : Framework.Abstractions.IQuery<int> { }

    private class TestHandler : Framework.Abstractions.IQueryHandler<TestQuery, int>
    {
        public Task<int> Handle(TestQuery query, CancellationToken cancellationToken) => Task.FromResult(42);
    }

    [Fact]
    public async Task QueryAsync_ResolvesHandler_AndReturnsResult()
    {
        var services = new ServiceCollection();
        services.AddScoped<Framework.Abstractions.IQueryHandler<TestQuery, int>, TestHandler>();
        var sp = services.BuildServiceProvider();

        var dispatcher = new Framework.Infrastructure.Queries.Dispatcher.QueryDispatcher(sp);
        var result = await dispatcher.QueryAsync<int>(new TestQuery());
        Assert.Equal(42, result);
    }
}

