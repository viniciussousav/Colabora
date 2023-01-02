using Colabora.Domain.Entities;
using Colabora.Domain.Enums;

namespace Colabora.Application.UseCases.CreateSocialAction.Models;

public record CreateSocialActionResponse(
    int Id,
    string Title,
    List<Interests> Fields,
    DateTime OccurrenceDate,
    States State,
    string Description,
    int OrganizationId,
    int VolunteerCreatorId,
    List<Volunteer> Organizers,
    List<Volunteer> Participants,
    DateTime CreatedAt);