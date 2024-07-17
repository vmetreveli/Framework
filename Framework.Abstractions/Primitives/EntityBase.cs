namespace Framework.Abstractions.Primitives;

public abstract class EntityBase<TId> : IEquatable<EntityBase<TId>>
    where TId : notnull
{
    protected EntityBase(TId id)
    {
        Id = id;
    }


    public TId Id { get; }

    public bool Equals(EntityBase<TId>? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;
        return Id.Equals(other.Id);
    }

    public static bool operator ==(EntityBase<TId> first, EntityBase<TId> second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static bool operator !=(EntityBase<TId> first, EntityBase<TId> second)
    {
        return !(first == second);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        if (obj is not EntityBase<TId> entity) return false;
        return Id.Equals(entity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}

// public abstract class EntityBase : IEquatable<EntityBase>
// {
//     // [Key]
//     // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//     //Guid
//     private readonly List<IDomainEvent> _domainEvents = new();
//
//     protected EntityBase(int id) =>
//         Id = id;
//
//     protected EntityBase()
//     {
//     }
//
//     public int Id { get; set; }
//
//     public DateTime CreatedOn { get; }
//     public DateTime? ModifiedOn { get; }
//
//     public DateTime? DeletedOn { get; }
//     public bool IsDeleted { get; } = false;
//     
//     [NotMapped]
//     public IEnumerable<IDomainEvent> DomainEvents =>
//         _domainEvents.AsReadOnly();
//
//     public bool Equals(EntityBase? other)
//     {
//         if (other is null) return false;
//
//         return ReferenceEquals(this, other) || Id == other.Id;
//     }
//
//
//     protected void AddDomainEvent(IDomainEvent domainEvent) =>
//         _domainEvents.Add(domainEvent);
//
//
//     public void ClearDomainEvents() =>
//         _domainEvents.Clear();
//
//     public static bool operator ==(EntityBase? a, EntityBase? b)
//     {
//         if (a is null && b is null) return true;
//
//         if (a is null || b is null) return false;
//
//         return a.Equals(b);
//     }
//
//     public static bool operator !=(EntityBase a, EntityBase? b) => !( a == b );
//
//     /// <inheritdoc />
//     public override bool Equals(object? obj)
//     {
//         if (obj is null) return false;
//
//         if (ReferenceEquals(this, obj)) return true;
//
//         if (obj.GetType() != GetType()) return false;
//
//         if (obj is not EntityBase other) return false;
//
//         return Id == other.Id;
//     }
//
//     /// <inheritdoc />
//     public override int GetHashCode() => Id.GetHashCode();
// }