﻿using Colabora.Application.Features.CreateSocialAction.Models;
using Colabora.Application.Features.GetSocialActions.Models;
using Colabora.Application.Features.JoinSocialAction;
using Colabora.Application.Features.JoinSocialAction.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
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
            : StatusCode(result.FailureStatusCode, result.Error);
    } 
    
    [HttpGet]
    public async Task<IActionResult> GetSocialActions()
    {
        var result = await _mediator.Send(new GetSocialActionsQuery());

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Error);
    }
    
    [HttpPost("{id:int}/join")]
    public async Task<IActionResult> JoinSocialAction([FromRoute]int id, [FromBody] JoinSocialActionCommand command)
    {
        if (id != command.SocialActionId)
            return BadRequest();
        
        var result = await _mediator.Send(command);
        
        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Error);
    } 
}