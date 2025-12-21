namespace Meadow_Framework.Framework.Infrastructure.Seed;

/// <summary>
///     Defines a contract for seeding data into the database.
/// </summary>
public interface IDataSeeder
{
    /// <summary>
    ///     Asynchronously seeds all required data into the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SeedAllAsync();
}