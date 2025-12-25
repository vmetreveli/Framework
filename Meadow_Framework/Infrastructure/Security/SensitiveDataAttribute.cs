namespace Meadow_Framework.Infrastructure.Security;

/// <summary>
///
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class SensitiveDataAttribute : Attribute
{
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