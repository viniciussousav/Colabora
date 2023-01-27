using Colabora.Application.Commons;
using Colabora.Application.Shared;
using Colabora.Application.Shared.Mappers;
using Colabora.Application.UseCases.GetSocialActions.Models;
using Colabora.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.UseCases.GetSocialActions;

public class GetSocialActionsQueryHandler : IGetSocialActionsQueryHandler
{
    private readonly ILogger<GetSocialActionsQueryHandler> _logger;
    private readonly ISocialActionRepository _socialActionRepository;

    public GetSocialActionsQueryHandler(ISocialActionRepository socialActionRepository, ILogger<GetSocialActionsQueryHandler> logger)
    {
        _socialActionRepository = socialActionRepository;
        _logger = logger;
    }

    public async Task<Result<GetSocialActionsResponse>> Handle(GetSocialActionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var socialActions = await _socialActionRepository.GetAllSocialActions();
            
            var response = new GetSocialActionsResponse(
                SocialActions: socialActions.Select(action => action.MapToGetSocialActionsItem()).ToList());
            
            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {GetSocialActionsQueryHandler}", nameof(GetSocialActionsQueryHandler));
            return Result.Fail<GetSocialActionsResponse>(ErrorMessages.CreateInternalError(e.Message));
        }
    }
}