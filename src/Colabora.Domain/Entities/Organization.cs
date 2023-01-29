using Colabora.Domain.Enums;

#pragma warning disable CS8618

namespace Colabora.Domain.Entities;

public class Organization
{
    public static readonly Organization None = new ();
    
    private Organization() { }
    
    public Organization(
        string name,
        string email,
        States state,
        List<Interests> interests,
        int createdBy)
    { 
        Name = name;
        Email = email;
        State = state;
        Interests = interests;
        CreatedBy = createdBy;
    }

    public int OrganizationId { get; }
    public string Name { get; }
    public string Email { get; }
    public States State { get; }
    public List<Interests> Interests { get; }
    public int CreatedBy { get; }
    public DateTime CreatedAt { get; }
    public IEnumerable<SocialAction> SocialActions { get; }
}