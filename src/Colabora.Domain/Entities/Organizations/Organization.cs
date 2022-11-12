using Colabora.Domain.Entities.Organizations.ValueObjects;
using Colabora.Domain.Entities.Volunteers;
using Colabora.Domain.Shared.Enums;

namespace Colabora.Domain.Entities.Organizations;

public class Organization
{
    public Organization(
        int id,
        string name,
        States state,
        List<Interests> interests,
        List<Membership> memberships,
        Volunteer createdBy,
        string createdAt)
    {
        Id = id;
        Name = name;
        State = state;
        Interests = interests;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        Memberships = memberships;
    }
    public int Id { get; }
    public string Name { get; }
    public States State { get; }
    public List<Interests> Interests { get; }
    public List<Membership> Memberships { get; }
    public Volunteer CreatedBy { get; }
    public string CreatedAt { get; }
}