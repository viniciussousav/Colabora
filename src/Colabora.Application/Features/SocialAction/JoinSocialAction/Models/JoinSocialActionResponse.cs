namespace Colabora.Application.Features.SocialAction.JoinSocialAction.Models;

public record JoinSocialActionResponse(
    string VolunteerName,
    string SocialActionName,
    DateTimeOffset JoinedAt);