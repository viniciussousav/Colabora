using Colabora.Application.Commons;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.Volunteer.GetVolunteers;

public class GetVolunteersQueryHandler : IGetVolunteersQueryHandler
{
    private readonly ILogger<GetVolunteersQueryHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;

    public GetVolunteersQueryHandler(ILogger<GetVolunteersQueryHandler> logger, IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<GetVolunteersResponse>> Handle(GetVolunteersQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var volunteers = await _volunteerRepository.GetAllVolunteers();
            var response = new GetVolunteersResponse(volunteers.Select(volunteer => volunteer.MapToGetVolunteerItemResponse()).ToList());
            
            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {GetVolunteersQueryHandler}", nameof(GetVolunteersQueryHandler));
            return Result.Fail<GetVolunteersResponse>(ErrorMessages.CreateInternalError(e.Message));
        }
    }
}