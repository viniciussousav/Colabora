using Colabora.Application.Shared;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

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
                return BadRequest(ErrorMessages.CreateInvalidOAuthToken(AuthProvider.Google));

            var authenticationResult = await _authService.Authenticate(AuthProvider.Google, idToken);

            if (!authenticationResult.IsValid)
                return Unauthorized(ErrorMessages.CreateInvalidOAuthToken(AuthProvider.Google, authenticationResult.Error));

            var notRegistered = await _volunteerRepository.GetVolunteerByEmail(authenticationResult.Email) == Volunteer.None;
            
            if (notRegistered)
                return NotFound(ErrorMessages.CreateVolunteerNotFound());
            
            return Ok(authenticationResult.Token);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.CreateInternalError(e.Message));
        }
    }
}