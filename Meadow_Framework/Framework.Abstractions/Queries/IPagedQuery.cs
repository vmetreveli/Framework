namespace Meadow_Framework.Framework.Abstractions.Queries;

/// <summary>
///     Represents a query that supports pagination.
/// </summary>
public interface IPagedQuery : IQuery
{
    /// <summary>
    ///     Gets or sets the page number to retrieve (typically 1-based).
    /// </summary>
    int Page { get; set; }

    /// <summary>
    ///     Gets or sets the number of results per page.
    /// </summary>
    int Results { get; set; }
}