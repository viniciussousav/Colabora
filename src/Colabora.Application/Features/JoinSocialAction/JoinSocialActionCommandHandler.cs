using Colabora.Application.Commons;
using Colabora.Application.Features.JoinSocialAction.Models;
using Colabora.Application.Shared;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.JoinSocialAction;

public class JoinSocialActionCommandHandler : IJoinSocialActionCommandHandler
{
    private readonly ILogger<JoinSocialActionCommandHandler> _logger;
    private readonly ISocialActionRepository _socialActionRepository;
    private readonly IVolunteerRepository _volunteerRepository;

    public JoinSocialActionCommandHandler(
        ILogger<JoinSocialActionCommandHandler> logger,
        ISocialActionRepository socialActionRepository,
        IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _socialActionRepository = socialActionRepository;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<JoinSocialActionResponse>> Handle(JoinSocialActionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var socialAction = await _socialActionRepository.GetSocialActionById(command.SocialActionId, cancellationToken);
        
            if(socialAction == SocialAction.None)
                return Result.Fail<JoinSocialActionResponse>(ErrorMessages.CreateSocialActionNotFound());
            
            if(!await VolunteerExists(command.VolunteerId))
                return Result.Fail<JoinSocialActionResponse>(ErrorMessages.CreateVolunteerNotFound());

            var participation = new Participation(command.SocialActionId, command.VolunteerId, DateTimeOffset.Now);
            socialAction.AddParticipation(participation);

            return Result.Success(new JoinSocialActionResponse());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {JoinSocialActionCommandHandler}", nameof(JoinSocialActionCommandHandler));
            return Result.Fail<JoinSocialActionResponse>(ErrorMessages.CreateInternalError(e.Message));
        }
    }
    
    private async Task<bool> VolunteerExists(int volunteerId)
        => await _volunteerRepository.GetVolunteerById(volunteerId) != Volunteer.None;
}