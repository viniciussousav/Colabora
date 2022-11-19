using Colabora.Application.Commons;
using Colabora.Application.Handlers.Volunteers.GetVolunteers.Models;
using Colabora.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Handlers.Volunteers.GetVolunteers;

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
            var volunteers = await _volunteerRepository.GetAllVolunteer();
            var response = new GetVolunteersResponse(volunteers.Select(volunteer => volunteer.MapToGetVolunteerResponse()).ToList());
            
            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {GetVolunteersQueryHandler}", nameof(GetVolunteersQueryHandler));
            return Result.Fail<GetVolunteersResponse>(ErrorMessages.CreateInternalErrorException(e.Message));
        }
    }
}