namespace Colabora.Domain.Shared.Interfaces;

public interface IEntity<out TKey>
{
    public TKey Id { get; }
}