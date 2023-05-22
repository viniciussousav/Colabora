using Colabora.Application.Features.Volunteer.GetVolunteerById.Models;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Application.Shared;
using Colabora.Domain.Shared;
using Colabora.Infrastructure.Auth;
using MediatR;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetVolunteerById([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetVolunteerByIdQuery(id));

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

        var authenticationResult = await authService.Authenticate(AuthProvider.Google, oAuthToken);

        if (!authenticationResult.IsValid)
            return Unauthorized(new List<Error> {ErrorMessages.CreateInvalidOAuthToken(AuthProvider.Google, authenticationResult.Error)});

        var commandWithEmail = command with {Email = authenticationResult.Email};

        var result = await _mediator.Send(commandWithEmail);

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }
}