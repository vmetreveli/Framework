namespace Meadow_Framework.Infrastructure.Security;

/// <summary>
///
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class SensitiveDataAttribute : Attribute
{
}