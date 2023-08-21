using Colabora.Application.Features.Volunteer.GetVolunteerById.Models;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Features.Volunteer.RegisterOrganization.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Shared.Errors;
using Colabora.Infrastructure.Auth;
using Colabora.Infrastructure.Auth.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ErrorMessages = Colabora.Application.Shared.ErrorMessages;

namespace Colabora.WebAPI.Controllers.v1;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/volunteers")]
public class VolunteerController : ControllerBase
{
    private readonly IMediator _mediator;

    public VolunteerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetVolunteers()
    {
        var result = await _mediator.Send(new GetVolunteersQuery());

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }

    [HttpGet("{volunteerId:guid}")]
    public async Task<IActionResult> GetVolunteerById([FromRoute] Guid volunteerId)
    {
        var result = await _mediator.Send(new GetVolunteerByIdQuery(volunteerId));

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterVolunteer(
        [FromServices] IAuthService authService,
        [FromHeader(Name = "OAuthToken")] string oAuthToken,
        [FromBody] RegisterVolunteerCommand command)
    {
        if (string.IsNullOrWhiteSpace(oAuthToken))
            return BadRequest(new List<Error> {ErrorMessages.CreateInvalidOAuthToken(AuthProvider.Google)});

        var authenticationResult = await authService.AuthenticateUser(AuthProvider.Google, oAuthToken);

        if (!authenticationResult.IsValid)
            return Unauthorized(new List<Error> {ErrorMessages.CreateInvalidOAuthToken(AuthProvider.Google, authenticationResult.Error)});

        command = command with {Email = authenticationResult.Email};

        var result = await _mediator.Send(command);

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }
    
    [Authorize(Policy = Policies.User)]
    [HttpPost("{volunteerId:guid}/organization")]
    public async Task<IActionResult> RegisterOrganization([FromQuery] Guid volunteerId, [FromBody] RegisterOrganizationCommand command)
    {
        var result = await _mediator.Send(command);

        return !result.IsValid 
            ? StatusCode(result.FailureStatusCode, result.Errors) 
            : CreatedAtRoute("GetOrganizationById", new {Id = result.Value?.OrganizationId}, result.Value);
    }
    
    [Authorize(Policy = Policies.User)]
    [HttpPut("{volunteerId:guid}/organization/{organizationId:guid}/join")]
    public Task<IActionResult> JoinOrganization(Guid volunteerId, Guid organizationId)
    {
        return Task.FromResult((IActionResult) Ok());
    }
    
    [Authorize(Policy = Policies.User)]
    [HttpPut("{volunteerId:guid}/organization/{organizationId:guid}/leave")]
    public Task<IActionResult> LeaveOrganization(Guid volunteerId, Guid organizationId)
    {
        return Task.FromResult((IActionResult) Ok());
    }
    
    [Authorize(Policy = Policies.User)]
    [HttpPost("{volunteerId:guid}/organization/{organizationId:guid}/actions")]
    public Task<IActionResult> CreateSocialAction(Guid volunteerId, Guid organizationId)
    {
        return Task.FromResult((IActionResult) Ok());
    }
    
    [Authorize(Policy = Policies.User)]
    [HttpPost("{volunteerId:guid}/organization/{organizationId:guid}/actions/{actionId:guid}/cancel")]
    public Task<IActionResult> CancelSocialAction(Guid volunteerId, Guid organizationId, Guid actionId)
    {
        return Task.FromResult((IActionResult) Ok());
    }
    
    [Authorize(Policy = Policies.User)]
    [HttpPost("{volunteerId:guid}/organization/{organizationId:guid}/actions/{actionId:guid}/join")]
    public Task<IActionResult> JoinSocialAction(Guid volunteerId, Guid organizationId, Guid actionId)
    {
        return Task.FromResult((IActionResult) Ok());
    }
    
    [Authorize(Policy = Policies.User)]
    [HttpPost("{volunteerId:guid}/organization/{organizationId:guid}/actions/{actionId:guid}/leave")]
    public Task<IActionResult> LeaveSocialAction(Guid volunteerId, Guid organizationId, Guid actionId)
    {
        return Task.FromResult((IActionResult) Ok());
    }
}