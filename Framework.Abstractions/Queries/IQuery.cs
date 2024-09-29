namespace Framework.Abstractions.Queries;

/// <summary>
///     Marker interface to represent a query without a result type.
/// </summary>
public interface IQuery
{
}

/// <summary>
///     Marker interface to represent a query with a result type.
/// </summary>
/// <typeparam name="T">The type of the result produced by the query.</typeparam>
public interface IQuery<T> : IQuery
{
}