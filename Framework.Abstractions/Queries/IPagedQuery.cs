namespace Framework.Abstractions.Queries;

public interface IPagedQuery : IQuery
{
    int Page { get; set; }
    int Results { get; set; }
}