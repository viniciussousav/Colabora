using Colabora.Domain.Shared.Enums;
using Colabora.Domain.Shared.Interfaces;

#pragma warning disable CS8618

namespace Colabora.Domain.Organization;

public class Organization : EntityBase<Guid>, IAggregateRoot
{
    public static readonly Organization None = new ();

    public Organization(
        Guid volunteerCreatorId,
        string name,
        string email,
        States state,
        IEnumerable<Interests> interests)
    {
        VolunteerCreatorId = volunteerCreatorId;
        Name = name;
        Email = email;
        State = state;
        Interests = interests;
    }

    private Organization() { }
    public string Name { get; }
    public string Email { get; }
    public States State { get; }
    public IEnumerable<Interests> Interests { get; }
    public Guid VolunteerCreatorId { get; }
    public DateTimeOffset CreatedAt { get; }
    public IEnumerable<SocialAction.SocialAction> SocialActions { get; }
    public bool Verified { get; private set; } = false;
}