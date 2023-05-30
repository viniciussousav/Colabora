using Colabora.Application.Commons;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Organization;
using Colabora.Domain.SocialAction;
using Colabora.Domain.Volunteer;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.SocialAction.CreateSocialAction;

public class CreateSocialActionCommandHandler : ICreateSocialActionCommandHandler
{
    private readonly ILogger<CreateSocialActionCommandHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ISocialActionRepository _socialActionRepository;
    private readonly IValidator<CreateSocialActionCommand> _validator;

    public CreateSocialActionCommandHandler(
        ILogger<CreateSocialActionCommandHandler> logger,
        IVolunteerRepository volunteerRepository,
        IOrganizationRepository organizationRepository,
        ISocialActionRepository socialActionRepository, 
        IValidator<CreateSocialActionCommand> validator)
    {
        _logger = logger;
        _socialActionRepository = socialActionRepository;
        _validator = validator;
        _volunteerRepository = volunteerRepository;
        _organizationRepository = organizationRepository;
    }
    
    public async Task<Result<CreateSocialActionResponse>> Handle(CreateSocialActionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Fail<CreateSocialActionResponse>(validationResult.Errors);

            if (!await OrganizationExists(command.OrganizationId))
            {
                return Result.Fail<CreateSocialActionResponse>(
                    error: ErrorMessages.CreateOrganizationNotFound(),
                    failureStatusCode: StatusCodes.Status404NotFound);
            }

            if (!await VolunteerCreatorExists(command.VolunteerCreatorId))
            {
                return Result.Fail<CreateSocialActionResponse>(
                    error: ErrorMessages.CreateVolunteerNotFound(),
                    failureStatusCode: StatusCodes.Status404NotFound);
            }

            var socialAction = await _socialActionRepository.CreateSocialAction(command.MapToSocialAction());

            return Result.Success(socialAction.MapToCreateSocialActionResponse());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {CreateSocialActionCommandHandler}", nameof(CreateSocialActionCommandHandler));
            return Result.Fail<CreateSocialActionResponse>(
                error: ErrorMessages.CreateInternalError(e.Message),
                failureStatusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
    private async Task<bool> OrganizationExists(int organizationId) =>
        await _organizationRepository.GetOrganizationById(organizationId) != Domain.Organization.Organization.None;

    private async Task<bool> VolunteerCreatorExists(int volunteerId) =>
        await _volunteerRepository.GetVolunteerById(volunteerId) != Domain.Volunteer.Volunteer.None;
}