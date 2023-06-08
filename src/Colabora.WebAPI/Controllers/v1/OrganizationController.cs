using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Features.Organization.VerifyOrganization.Models;
using Colabora.Application.Services.EmailVerification;
using Colabora.Application.Shared;
using Colabora.Domain.Organization;
using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers.v1;

[Authorize]
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/organizations")]
public class OrganizationController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrganizationById([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetOrganizationByIdQuery(id));

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterOrganization([FromBody] RegisterOrganizationCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsValid)
            return StatusCode(result.FailureStatusCode, result.Errors);

        return CreatedAtAction(nameof(GetOrganizationById), new {Id = result.Value?.OrganizationId}, result.Value);
    }

    [HttpPut("{id:int}/verification/confirm")]
    public async Task<IActionResult> ConfirmOrganizationVerification([FromRoute] int id, [FromQuery] Guid verificationCode)
    {
        var result = await _mediator.Send(new VerifyOrganizationCommand(id, verificationCode));

        if (!result.IsValid)
            return StatusCode(result.FailureStatusCode, result.Errors);
        
        return Ok();
    }
    
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