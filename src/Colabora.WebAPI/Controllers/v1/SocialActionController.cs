using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Features.SocialAction.GetSocialActionById.Models;
using Colabora.Application.Features.SocialAction.GetSocialActions.Models;
using Colabora.Application.Features.SocialAction.JoinSocialAction.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers.v1;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/actions")]
public class SocialActionController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public SocialActionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSocialAction(CreateSocialActionCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    } 
    
    [HttpGet]
    public async Task<IActionResult> GetSocialActions()
    {
        var result = await _mediator.Send(new GetSocialActionsQuery());

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }
    
    [HttpGet("{Id:int}")]
    public async Task<IActionResult> GetSocialActionById([FromRoute] GetSocialActionByIdQuery query)
    {
        var result = await _mediator.Send(query);

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }
    
    [HttpPost("{id:int}/join")]
    public async Task<IActionResult> JoinSocialAction([FromRoute]int id, [FromBody] JoinSocialActionCommand command)
    {
        if (id != command.SocialActionId)
            return BadRequest();
        
        var result = await _mediator.Send(command);
        
        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    } 
}