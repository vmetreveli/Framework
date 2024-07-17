namespace Framework.Abstractions.Primitives;

public interface IAuditableEntity
{
    DateTime CreatedOn { get; }
    DateTime ModifiedOn { get; }
}