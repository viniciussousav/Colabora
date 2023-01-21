using Colabora.Domain.Enums;
#pragma warning disable CS8618

namespace Colabora.Domain.Entities;

public class SocialAction
{
    public static readonly SocialAction None = new();

    private SocialAction() { }
    
    public SocialAction(
        string title,
        string description,
        int organizationId,
        int volunteerCreatorId,
        States state,
        List<Interests> interests,
        DateTimeOffset occurrenceDate,
        DateTimeOffset createdAt)
    {
        Title = title;
        Description = description;
        OrganizationId = organizationId;
        VolunteerCreatorId = volunteerCreatorId;
        State = state;
        Interests = interests;
        OccurrenceDate = occurrenceDate;
        CreatedAt = createdAt;
    }
    
    public int SocialActionId { get; }
    public string Title { get; }
    public string Description { get; }
    public int OrganizationId { get; }
    public Organization Organization { get; }
    public int VolunteerCreatorId { get; }
    public States State { get; }
    public List<Interests> Interests { get; }
    public DateTimeOffset OccurrenceDate { get; }
    public DateTimeOffset CreatedAt { get; }
}