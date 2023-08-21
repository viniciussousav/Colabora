namespace Colabora.Domain.Shared.Interfaces;

public class EntityBase <TKey> : IEntity<TKey> where TKey : struct
{
    public TKey Id { get; protected set; }

    public override bool Equals(object? obj)
    {
        return GetType() == obj?.GetType() && Id.Equals((obj as EntityBase<TKey>)?.Id);
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(EntityBase<TKey> left, EntityBase<TKey> right)
    {
        return left.Id.Equals(right.Id);
    }
    
    public static bool operator !=(EntityBase<TKey> left, EntityBase<TKey> right)
    {
        return !left.Id.Equals(right.Id);
    }
}