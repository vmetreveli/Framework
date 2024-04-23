namespace Framework.Abstractions.Kernel.Types;

public class EntityId(Guid value) : TypeId(value)
{
    public static implicit operator EntityId(Guid id) => new(id);

    public static implicit operator Guid(EntityId id) => id.Value;
}