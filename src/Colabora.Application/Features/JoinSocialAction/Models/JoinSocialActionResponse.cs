namespace Colabora.Application.Features.JoinSocialAction.Models;

public record JoinSocialActionResponse(
    string VolunteerName,
    string SocialActionName,
    DateTimeOffset JoinedAt);