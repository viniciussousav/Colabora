using Colabora.Domain.Enums;

namespace Colabora.Domain.Entities;

public class Organization
{
    private Organization() { }

    public static readonly Organization Empty = new ();

    public Organization(
        int id,
        string name,
        string email,
        States state,
        List<Interests> interests,
        List<Membership> memberships,
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
        Memberships = memberships;
    }

    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public States State { get; }
    public List<Interests> Interests { get; }
    public List<Membership> Memberships { get; }
    public int CreatedBy { get; }
    public DateTime CreatedAt { get; }
}