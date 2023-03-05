using Colabora.Application.Commons;
using Colabora.Application.Features.GetVolunteerById.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.GetVolunteerById;

public class GetVolunteerByIdQueryHandler : IGetVolunteerByIdQueryHandler
{
    private readonly ILogger<GetVolunteerByIdQueryHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;

    public GetVolunteerByIdQueryHandler(ILogger<GetVolunteerByIdQueryHandler> logger, IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<GetVolunteerByIdResponse>> Handle(GetVolunteerByIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var volunteer = await _volunteerRepository.GetVolunteerById(query.Id, includeParticipations: true);

            if (volunteer == Volunteer.None)
                return Result.Fail<GetVolunteerByIdResponse>(ErrorMessages.CreateVolunteerNotFound());

            var response = volunteer.MapToGetVolunteerByIdResponse();
            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {GetVolunteerByIdQueryHandler}", nameof(GetVolunteerByIdQueryHandler));
            return Result.Fail<GetVolunteerByIdResponse>(ErrorMessages.CreateInternalError(e.Message));
        }
    }
}