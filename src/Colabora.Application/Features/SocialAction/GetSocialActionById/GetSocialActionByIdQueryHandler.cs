using Colabora.Application.Commons;
using Colabora.Application.Features.SocialAction.GetSocialActionById.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.SocialAction.GetSocialActionById;

public class GetSocialActionByIdQueryHandler : IGetSocialActionByIdQueryHandler
{
    private readonly ILogger<GetSocialActionByIdQueryHandler> _logger;
    private readonly ISocialActionRepository _socialActionRepository;

    public GetSocialActionByIdQueryHandler(ILogger<GetSocialActionByIdQueryHandler> logger, ISocialActionRepository socialActionRepository)
    {
        _logger = logger;
        _socialActionRepository = socialActionRepository;
    }

    public async Task<Result<GetSocialActionByIdResponse>> Handle(GetSocialActionByIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var socialAction = await _socialActionRepository.GetSocialActionById(query.Id, cancellationToken);

            if (socialAction == Domain.Entities.SocialAction.None)
                return Result.Fail<GetSocialActionByIdResponse>(
                    error: ErrorMessages.CreateSocialActionNotFound(),
                    failureStatusCode: StatusCodes.Status404NotFound);

            var response = socialAction.MapToGetSocialActionByIdResponse();
            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {GetSocialActionByIdQueryHandler}", nameof(GetSocialActionByIdQueryHandler));
            return Result.Fail<GetSocialActionByIdResponse>(
                error: ErrorMessages.CreateInternalError(e.Message),
                failureStatusCode: StatusCodes.Status500InternalServerError);
        }
    }
}