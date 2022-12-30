using Colabora.Domain.Entities;
using Colabora.Domain.Enums;

namespace Colabora.Application.UseCases.CreateSocialAction.Models;

public record CreateSocialActionResponse(
    int Id,
    string Title,
    Interests Fields,
    DateTime OccurrenceDate,
    string State,
    string Description,
    int OrganizationId,
    int VolunteerCreatorId,
    List<Volunteer> Organizers,
    List<Volunteer> Participants,
    DateTime CreatedAt);