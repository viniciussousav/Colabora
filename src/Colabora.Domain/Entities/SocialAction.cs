using Colabora.Domain.Enums;

namespace Colabora.Domain.Entities;

public class SocialAction
{
    public SocialAction(
        string title,
        Interests fields,
        DateTime occurrenceDate,
        string state,
        string description,
        Organization organization,
        Volunteer createdBy,
        List<Volunteer> organizers,
        List<Volunteer> participants,
        DateTime createdAt)
    {
        Title = title;
        Fields = fields;
        OccurrenceDate = occurrenceDate;
        State = state;
        Description = description;
        Organization = organization;
        Organizers = organizers;
        Participants = participants;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }

    public string Title { get; }
    public Interests Fields { get; }
    public DateTime OccurrenceDate { get; }
    public string State { get; }
    public string Description { get; }
    public Organization Organization { get; }
    public Volunteer CreatedBy { get; }
    public List<Volunteer> Organizers { get; }
    public List<Volunteer> Participants { get; }
    public DateTime CreatedAt { get; }
}