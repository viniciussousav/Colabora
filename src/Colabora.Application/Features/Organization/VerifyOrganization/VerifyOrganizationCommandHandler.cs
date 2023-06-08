using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.VerifyOrganization.Models;
using Colabora.Application.Services.EmailVerification;
using Colabora.Domain.Organization;
using Colabora.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ErrorMessages = Colabora.Application.Shared.ErrorMessages;

namespace Colabora.Application.Features.Organization.VerifyOrganization;

public class VerifyOrganizationCommandHandler : IVerifyOrganizationCommandHandler
{
    private readonly ILogger<VerifyOrganizationCommandHandler> _logger;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IEmailVerificationService _emailVerificationService;

    public VerifyOrganizationCommandHandler(
        ILogger<VerifyOrganizationCommandHandler> logger,
        IOrganizationRepository organizationRepository,
        IEmailVerificationService emailVerificationService)
    {
        _logger = logger;
        _organizationRepository = organizationRepository;
        _emailVerificationService = emailVerificationService;
    }

    public async Task<Result<VerifyOrganizationResponse>> Handle(VerifyOrganizationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var organization = await _organizationRepository.GetOrganizationById(command.OrganizationId);

            if (organization == Domain.Organization.Organization.None)
                return Result.Fail<VerifyOrganizationResponse>(ErrorMessages.CreateOrganizationNotFound(), StatusCodes.Status404NotFound);

            await _emailVerificationService.ValidateEmailVerification(command.VerificationCode);
            
            organization.Verify();
            await _organizationRepository.UpdateOrganization(organization);

            return Result.Success(new VerifyOrganizationResponse());
        }
        catch (DomainException e)
        {
            return Result.Fail<VerifyOrganizationResponse>(e.Error, StatusCodes.Status400BadRequest);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected exception was throw at {VerifyOrganizationCommandHandler}", nameof(VerifyOrganizationCommandHandler));
            return Result.Fail<VerifyOrganizationResponse>(ErrorMessages.CreateInternalError(e.Message));
        }
    }
}