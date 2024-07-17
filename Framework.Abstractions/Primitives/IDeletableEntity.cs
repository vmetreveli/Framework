namespace Framework.Abstractions.Primitives;

public interface IDeletableEntity
{
    bool IsDeleted { get; }
    DateTime? DeletedOn { get; }
}