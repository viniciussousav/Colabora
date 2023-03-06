using Colabora.Application.Commons;
using Colabora.Application.Features.RegisterOrganization.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.RegisterOrganization;

public class RegisterOrganizationCommandHandler : IRegisterOrganizationCommandHandler
{
    private readonly ILogger<RegisterOrganizationCommandHandler> _logger;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IVolunteerRepository _volunteerRepository;

    public RegisterOrganizationCommandHandler(
        ILogger<RegisterOrganizationCommandHandler> logger,
        IOrganizationRepository organizationRepository, 
        IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _organizationRepository = organizationRepository;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<RegisterOrganizationResponse>> Handle(RegisterOrganizationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (await OrganizationEmailAlreadyExist(command.Name, command.Email, command.VolunteerCreatorId))
                return Result.Fail<RegisterOrganizationResponse>(ErrorMessages.CreateOrganizationEmailAlreadyExists(command.Name));

            if (await VolunteerCreatorNotExists(command.VolunteerCreatorId))
                return Result.Fail<RegisterOrganizationResponse>(ErrorMessages.CreateVolunteerNotFound());

            var organization = command.MapToOrganization();
            var createdOrganization = await _organizationRepository.CreateOrganization(organization);
            
            var response = createdOrganization.MapToRegisterOrganizationResponse();

            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {CreateOrganizationHandler}", nameof(RegisterOrganizationCommandHandler));
            return Result.Fail<RegisterOrganizationResponse>(ErrorMessages.CreateInternalError(e.Message));
        }
    }

    private async Task<bool> VolunteerCreatorNotExists(int volunteerId)
        => await _volunteerRepository.GetVolunteerById(volunteerId) == Volunteer.None;
    
    private async Task<bool> OrganizationEmailAlreadyExist(string name, string email, int volunteerCreator)
        => await _organizationRepository.GetOrganization(name, email, volunteerCreator) != Organization.None;

}