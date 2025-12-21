using AutoMapper;

namespace Meadow_Framework.Framework.Abstractions.Mappings;

/// <summary>
///     Defines a contract for configuring AutoMapper mappings.
/// </summary>
public interface IMap
{
    /// <summary>
    ///     Configures mappings for the specified AutoMapper profile.
    /// </summary>
    /// <param name="profile">The AutoMapper profile to configure.</param>
    void Mapping(Profile profile);
}