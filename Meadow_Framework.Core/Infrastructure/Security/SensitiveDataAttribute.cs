namespace Meadow_Framework.Core.Infrastructure.Security;

/// <summary>
///
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class SensitiveDataAttribute : Attribute
{
    /// <summary>
    ///
    /// </summary>
    public string Mask { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="mask"></param>
    public SensitiveDataAttribute(string mask = "****")
    {
        Mask = mask;
    }
}