using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.Domain.Shared;
using Colabora.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

using ErrorMessages = Colabora.Application.Shared.ErrorMessages;

namespace Colabora.WebAPI.Controllers.v1;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/login")]
public class LoginController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IVolunteerRepository _volunteerRepository;

    public LoginController(IAuthService authService, IVolunteerRepository volunteerRepository)
    {
        _authService = authService;
        _volunteerRepository = volunteerRepository;
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin([FromHeader(Name = "OAuthToken")] string idToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(idToken))
                return BadRequest(new List<Error> {ErrorMessages.CreateInvalidOAuthToken(AuthProvider.Google)});

            var authenticationResult = await _authService.Authenticate(AuthProvider.Google, idToken);

            if (!authenticationResult.IsValid)
                return Unauthorized(new List<Error> {ErrorMessages.CreateInvalidOAuthToken(AuthProvider.Google, authenticationResult.Error)});

            var volunteer = await _volunteerRepository.GetVolunteerByEmail(authenticationResult.Email);

            if (volunteer == Volunteer.None)
                return NotFound(new List<Error> {ErrorMessages.CreateVolunteerNotFound()});

            return Ok(authenticationResult);
        }
        catch (Exception e)
        {
            return StatusCode(
                statusCode: StatusCodes.Status500InternalServerError,
                value: new List<Error> {ErrorMessages.CreateInternalError(e.Message)});
        }
    }
}