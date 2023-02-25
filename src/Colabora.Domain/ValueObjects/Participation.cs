using Colabora.Domain.Entities;

namespace Colabora.Domain.ValueObjects;

public class Participation
{
    private static readonly Participation None = new();
    
    private Participation(){ }

    public Participation(int socialActionId, int volunteerId, DateTimeOffset joinedAt)
    {
        SocialActionId = socialActionId;
        VolunteerId = volunteerId;
        JoinedAt = joinedAt;
    }
    
    public int SocialActionId { get; }
    
    public Volunteer Volunteer { get; }
    public int VolunteerId { get; }
    public DateTimeOffset JoinedAt { get; }
}