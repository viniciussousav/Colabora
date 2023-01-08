using Colabora.Application.Shared;
using Colabora.Application.UseCases.GetVolunteers.Models;
using Colabora.Application.UseCases.RegisterVolunteer.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers;

[ApiController]
[Route("volunteers")]
public class VolunteerController : ControllerBase
{
    private readonly ILogger<VolunteerController> _logger;
    private readonly IMediator _mediator;
    
    public VolunteerController(ILogger<VolunteerController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var result = await _mediator.Send(new GetVolunteersQuery());

            return result.IsValid  
                ? Ok(result.Value)
                : StatusCode(result.FailureStatusCode, result.Error);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {VolunteerController}", nameof(VolunteerController));
            return Problem(detail: e.Message, statusCode: 500, title: "Internal Error");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(RegisterVolunteerCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            
            if (result.Error.Code == ErrorMessages.VolunteerEmailAlreadyRegistered)
                return Conflict(result.Error);
            
            return Ok(result.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {VolunteerController}", nameof(VolunteerController));
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: ErrorMessages.InternalError, detail: e.Message);
        }
    }
}