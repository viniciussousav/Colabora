using System.Text.Json.Serialization;
using Colabora.Domain.Shared.Enums;

namespace Colabora.Application.Features.SocialAction.CreateSocialAction.Models;

public record CreateSocialActionResponse
{
    public CreateSocialActionResponse(int SocialActionId,
        string Title,
        string Description,
        int OrganizationId,
        int VolunteerCreatorId,
        States State,
        List<Interests> Interests,
        DateTimeOffset OccurrenceDate,
        DateTimeOffset CreatedAt)
    {
        this.SocialActionId = SocialActionId;
        this.Title = Title;
        this.Description = Description;
        this.OrganizationId = OrganizationId;
        this.VolunteerCreatorId = VolunteerCreatorId;
        this.State = State;
        this.Interests = Interests;
        this.OccurrenceDate = OccurrenceDate;
        this.CreatedAt = CreatedAt;
    }

    public int SocialActionId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public int OrganizationId { get; init; }
    public int VolunteerCreatorId { get; init; }
    public States State { get; init; }
    public List<Interests> Interests { get; init; }
    public DateTimeOffset OccurrenceDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}