using System.Threading;
using System.Threading.Tasks;
using Framework.Infrastructure.Dispatchers;
using Moq;
using Xunit;

namespace Framework.Infrastructure.Tests.Dispatchers;

public class DispatcherTests
{
    private class TestCommand : Framework.Abstractions.ICommand { }

    [Fact]
    public async Task SendAsync_DelegatesToCommandDispatcher()
    {
        var mockCmd = new Mock<Framework.Abstractions.ICommandDispatcher>();
        var mockQry = new Mock<Framework.Abstractions.IQueryDispatcher>();
        var dispatcher = new Dispatcher(mockCmd.Object, mockQry.Object);

        var cmd = new TestCommand();
        await dispatcher.SendAsync(cmd);

        mockCmd.Verify(m => m.SendAsync(It.IsAny<Framework.Abstractions.ICommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

