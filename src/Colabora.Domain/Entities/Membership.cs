namespace Colabora.Domain.Entities;

public class Membership
{
    private static readonly Membership None = new();
    private Membership() { }
    
    public Membership(int organizationId, int volunteerId, bool active, DateTime createdAt)
    {
        OrganizationId = organizationId;
        VolunteerId = volunteerId;
        Active = active;
        CreatedAt = createdAt;
    }

    public int MembershipId { get; }
    public int OrganizationId { get; }
    public int VolunteerId { get; }
    public bool Active { get; }
    public DateTime CreatedAt { get; }

}