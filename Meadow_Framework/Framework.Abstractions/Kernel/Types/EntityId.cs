namespace Meadow_Framework.Framework.Abstractions.Kernel.Types;

public class EntityId(Guid value) : TypeId(value)
{
    public static implicit operator EntityId(Guid id)
    {
        return new EntityId(id);
    }

    public static implicit operator Guid(EntityId id)
    {
        return id.Value;
    }
}