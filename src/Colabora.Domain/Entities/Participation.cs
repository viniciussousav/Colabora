#pragma warning disable CS8618

namespace Colabora.Domain.Entities;

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
    
    public SocialAction SocialAction { get; }
    public int SocialActionId { get; }
    public Volunteer Volunteer { get; }
    public int VolunteerId { get; }
    public DateTimeOffset JoinedAt { get; }
}