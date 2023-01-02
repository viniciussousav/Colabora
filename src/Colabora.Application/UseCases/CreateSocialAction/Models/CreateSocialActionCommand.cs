using Colabora.Application.Commons;
using Colabora.Domain.Entities;
using Colabora.Domain.Enums;
using MediatR;

namespace Colabora.Application.UseCases.CreateSocialAction.Models;

public record CreateSocialActionCommand(
        string Title,
        List<Interests> Fields,
        DateTime OccurrenceDate,
        States State,
        string Description,
        int OrganizationId,
        int VolunteerCreatorId,
        List<Volunteer> Organizers,
        List<Volunteer> Participants) 
    : IRequest<Result<CreateSocialActionResponse>>;