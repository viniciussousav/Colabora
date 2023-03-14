using Colabora.Application.Commons;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.SocialAction.CreateSocialAction;

public class CreateSocialActionCommandHandler : ICreateSocialActionCommandHandler
{
    private readonly ILogger<CreateSocialActionCommandHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ISocialActionRepository _socialActionRepository;
    
    public CreateSocialActionCommandHandler(
        ILogger<CreateSocialActionCommandHandler> logger,
        IVolunteerRepository volunteerRepository,
        IOrganizationRepository organizationRepository,
        ISocialActionRepository socialActionRepository)
    {
        _logger = logger;
        _socialActionRepository = socialActionRepository;
        _volunteerRepository = volunteerRepository;
        _organizationRepository = organizationRepository;
    }
    
    public async Task<Result<CreateSocialActionResponse>> Handle(CreateSocialActionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (!await OrganizationExists(command.OrganizationId))
                return Result.Fail<CreateSocialActionResponse>(ErrorMessages.CreateOrganizationNotFound());
        
            if (!await VolunteerCreatorExists(command.VolunteerCreatorId))
                return Result.Fail<CreateSocialActionResponse>(ErrorMessages.CreateVolunteerNotFound());

            var socialAction = await _socialActionRepository.CreateSocialAction(command.MapToSocialAction());
        
            return Result.Success(socialAction.MapToCreateSocialActionResponse());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {CreateSocialActionCommandHandler}", nameof(CreateSocialActionCommandHandler));
            return Result.Fail<CreateSocialActionResponse>(ErrorMessages.CreateInternalError(e.Message));
        }
    }
    
    private async Task<bool> OrganizationExists(int organizationId) =>
        await _organizationRepository.GetOrganizationById(organizationId) != Domain.Entities.Organization.None;

    private async Task<bool> VolunteerCreatorExists(int volunteerId) =>
        await _volunteerRepository.GetVolunteerById(volunteerId) != Domain.Entities.Volunteer.None;
}