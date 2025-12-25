using Meadow_Framework.Abstractions.Primitives;

namespace Meadow_Framework.Infrastructure;

/// <summary>
///
/// </summary>
public class BasketCheckoutIntegrationEvent: IntegrationBaseEvent
{
    /// <summary>
    ///
    /// </summary>
    public string Name { get; set; } = default!;

}