using Colabora.Application.Features.Organization.VerifyOrganization.Models;
using Colabora.Application.Services.EmailVerification;
using Colabora.Application.Shared;
using Colabora.Domain.Organization;
using Colabora.Infrastructure.Auth.Shared;
using Colabora.Infrastructure.Persistence.Repositories.EmailVerification.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers.v1;

public class EmailVerificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmailVerificationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(Policy = PolicyNames.EmailVerification)]
    [HttpPut("{id:int}/verification/confirm")]
    public async Task<IActionResult> ConfirmOrganizationVerification([FromRoute] int id, [FromQuery] Guid verificationCode)
    {
        var result = await _mediator.Send(new VerifyOrganizationCommand(id, verificationCode));

        if (!result.IsValid)
            return StatusCode(result.FailureStatusCode, result.Errors);
        
        return Ok();
    }
    
    [Authorize(Policy = PolicyNames.EmailVerification)]
    [HttpPut("{id:int}/verification/resend")]
    public async Task<IActionResult> ResendOrganizationVerification(
        [FromRoute] int id, 
        [FromServices] IEmailVerificationService emailVerification,
        [FromServices] IOrganizationRepository organizationRepository)
    {
        var organization = await organizationRepository.GetOrganizationById(id);

        if (organization == Organization.None)
            return BadRequest(ErrorMessages.CreateOrganizationNotFound());
        
        await emailVerification.SendEmailVerification(new EmailVerificationRequest(organization.Email));
        
        return Ok();
    }
}