using Colabora.Domain.Enums;

namespace Colabora.Domain.Entities;

public class SocialAction
{
    private static readonly SocialAction None = new();
    
    private SocialAction() { }
    
    public SocialAction(
        int id,
        string title,
        List<Interests> fields,
        DateTime occurrenceDate,
        States state,
        string description,
        int organizationId,
        int volunteerCreatorId,
        List<Volunteer> organizers,
        List<Volunteer> participants,
        DateTime createdAt)
    {
        Id = id;
        Title = title;
        Fields = fields;
        OccurrenceDate = occurrenceDate;
        State = state;
        Description = description;
        OrganizationId = organizationId;
        Organizers = organizers;
        Participants = participants;
        CreatedAt = createdAt;
        VolunteerCreatorId = volunteerCreatorId;
    }

    public int Id { get; }
    public string Title { get; }
    public List<Interests> Fields { get; }
    public DateTime OccurrenceDate { get; }
    public States State { get; }
    public string Description { get; }
    public int OrganizationId { get; }
    public int VolunteerCreatorId { get; }
    public List<Volunteer> Organizers { get; }
    public List<Volunteer> Participants { get; }
    public DateTime CreatedAt { get; }
}