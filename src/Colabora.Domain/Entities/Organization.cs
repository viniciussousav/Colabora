using Colabora.Domain.Enums;

namespace Colabora.Domain.Entities;

public class Organization
{
    protected Organization() { }

    public static readonly Organization None = new ();

    public Organization(
        int id,
        string name,
        string email,
        States state,
        List<Interests> interests,
        int createdBy,
        DateTime createdAt)
    {
        Id = id;
        Name = name;
        Email = email;
        State = state;
        Interests = interests;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
    }

    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public States State { get; }
    public List<Interests> Interests { get; }
    public int CreatedBy { get; }
    public DateTime CreatedAt { get; }
}