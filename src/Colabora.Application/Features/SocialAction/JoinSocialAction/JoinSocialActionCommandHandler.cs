using Colabora.Application.Commons;
using Colabora.Application.Features.SocialAction.JoinSocialAction.Models;
using Colabora.Application.Shared;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.SocialAction.JoinSocialAction;

public class JoinSocialActionCommandHandler : IJoinSocialActionCommandHandler
{
    private readonly ILogger<JoinSocialActionCommandHandler> _logger;
    private readonly ISocialActionRepository _socialActionRepository;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<JoinSocialActionCommand> _validator;

    public JoinSocialActionCommandHandler(
        ILogger<JoinSocialActionCommandHandler> logger,
        ISocialActionRepository socialActionRepository,
        IVolunteerRepository volunteerRepository, 
        IValidator<JoinSocialActionCommand> validator)
    {
        _logger = logger;
        _socialActionRepository = socialActionRepository;
        _volunteerRepository = volunteerRepository;
        _validator = validator;
    }

    public async Task<Result<JoinSocialActionResponse>> Handle(JoinSocialActionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Fail<JoinSocialActionResponse>(validationResult.Errors);
            
            var socialAction = await _socialActionRepository.GetSocialActionById(command.SocialActionId, cancellationToken);

            if (socialAction == Domain.Entities.SocialAction.None)
            {
                return Result.Fail<JoinSocialActionResponse>(
                    error: ErrorMessages.CreateSocialActionNotFound(),
                    failureStatusCode: StatusCodes.Status404NotFound);
            }

            var volunteer = await _volunteerRepository.GetVolunteerById(command.VolunteerId);

            if (volunteer == Domain.Entities.Volunteer.None)
            {
                return Result.Fail<JoinSocialActionResponse>(
                    error: ErrorMessages.CreateVolunteerNotFound(),
                    failureStatusCode: StatusCodes.Status404NotFound);
            }

            if (socialAction.Participations.Exists(participation => participation.VolunteerId == command.VolunteerId))
            {
                return Result.Fail<JoinSocialActionResponse>(
                    error: ErrorMessages.CreateJoinSocialActionConflict(),
                    failureStatusCode: StatusCodes.Status409Conflict);
            }

            var newParticipation = await CreateParticipation(socialAction.SocialActionId, command.VolunteerId);
    
            var response = new JoinSocialActionResponse(
                VolunteerName: volunteer.FullName,
                SocialActionName: socialAction.Title,
                newParticipation.JoinedAt);
            
            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {JoinSocialActionCommandHandler}", nameof(JoinSocialActionCommandHandler));
            return Result.Fail<JoinSocialActionResponse>(
                error: ErrorMessages.CreateInternalError(e.Message),
                failureStatusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
    private async Task<Participation> CreateParticipation(int socialActionId, int volunteerId)
    {
        var participation = new Participation(socialActionId, volunteerId, DateTimeOffset.UtcNow);
        await _socialActionRepository.CreateParticipation(socialActionId, participation);
        return participation;
    }
}