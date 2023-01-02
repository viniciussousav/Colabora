using Colabora.Application.Commons;
using Colabora.Application.Shared;
using Colabora.Application.Shared.Mappers;
using Colabora.Application.UseCases.CreateSocialAction.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;

namespace Colabora.Application.UseCases.CreateSocialAction;

public class CreateSocialActionCommandHandler : ICreateSocialActionCommandHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ISocialActionRepository _socialActionRepository;
    
    public CreateSocialActionCommandHandler(
        IVolunteerRepository volunteerRepository,
        IOrganizationRepository organizationRepository,
        ISocialActionRepository socialActionRepository)
    {
        _volunteerRepository = volunteerRepository;
        _organizationRepository = organizationRepository;
        _socialActionRepository = socialActionRepository;
    }

    public async Task<Result<CreateSocialActionResponse>> Handle(CreateSocialActionCommand command, CancellationToken cancellationToken)
    {
        if(await VolunteerExists(command.VolunteerCreatorId))
            return Result.Fail<CreateSocialActionResponse>(ErrorMessages.CreateVolunteerNotFound());
        
        if(await OrganizationExists(command.OrganizationId))
            return Result.Fail<CreateSocialActionResponse>(ErrorMessages.CreateOrganizationNotFound());

        var socialAction = command.MapToSocialAction();
        await _socialActionRepository.CreateSocialAction(socialAction);
        
        return Result.Success(socialAction.MapToResponse());
    }

    private async Task<bool> VolunteerExists(int volunteerId) =>
        await _volunteerRepository.GetVolunteerById(volunteerId) == Volunteer.None;

    private async Task<bool> OrganizationExists(int organizationId) =>
        await _organizationRepository.GetOrganizationById(organizationId) == Organization.None;
}