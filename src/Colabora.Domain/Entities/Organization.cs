using Colabora.Domain.Enums;
using Colabora.Domain.Shared;

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
        IEnumerable<Interests> interests,
        int createdBy,
        bool verified)
    { 
        Name = name;
        Email = email;
        State = state;
        Interests = interests;
        CreatedBy = createdBy;
        Verified = verified;
    }

    public int OrganizationId { get; }
    public string Name { get; }
    public string Email { get; }
    public States State { get; }
    public IEnumerable<Interests> Interests { get; }
    public int CreatedBy { get; }
    public DateTimeOffset CreatedAt { get; }
    public IEnumerable<SocialAction> SocialActions { get; }
    public bool Verified { get; private set; }

    public void Verify()
    {
        if (Verified)
            throw new DomainException(ErrorMessages.CreateOrganizationAlreadyVerified(OrganizationId));
        
        Verified = true;
    }
}