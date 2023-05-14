﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Colabora.Application.Features.SocialAction.GetSocialActions;
using Colabora.Application.Features.SocialAction.GetSocialActions.Models;
using Colabora.Application.Mappers;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.TestCommons.Fakers.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Colabora.UnitTests.Application.Features;

public class GetSocialActionsQueryHandlerTests
{
    private readonly ILogger<GetSocialActionsQueryHandler> _logger;
    private readonly ISocialActionRepository _socialActionRepository;

    public GetSocialActionsQueryHandlerTests()
    {
        _socialActionRepository = Substitute.For<ISocialActionRepository>();
        _logger = Substitute.For<ILogger<GetSocialActionsQueryHandler>>();
    }

    [Fact]
    public async Task Given_A_Query_When_There_Is_No_Social_Actions_Then_It_Should_Return_An_Empty_List()
    {
        // Arrange
        _socialActionRepository.GetAllSocialActions().Returns(new List<SocialAction>());

        var handler = new GetSocialActionsQueryHandler(_logger, _socialActionRepository);
        
        // Act
        var result = await handler.Handle(new GetSocialActionsQuery(), CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.SocialActions.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Given_A_Query_When_There_Are_Social_Actions_Then_It_Should_Return_A_List_With_Social_Actions()
    {
        // Arrange
        var socialActions = new List<SocialAction>
        {
            FakeSocialAction.Create(),
            FakeSocialAction.Create()
        };
        _socialActionRepository.GetAllSocialActions().Returns(socialActions);

        var handler = new GetSocialActionsQueryHandler(_logger, _socialActionRepository);
        
        // Act
        var result = await handler.Handle(new GetSocialActionsQuery(), CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.SocialActions.Should().HaveCount(2);
        result.Value.SocialActions[0].Should().BeEquivalentTo(socialActions[0].MapToGetSocialActionsItem());
        result.Value.SocialActions[1].Should().BeEquivalentTo(socialActions[1].MapToGetSocialActionsItem());
    }
    
    [Fact]
    public async Task Given_A_Query_When_An_Exception_Occurs_Then_It_Should_Return_An_Error()
    {
        // Arrange
        _socialActionRepository.GetAllSocialActions().Throws(new TaskCanceledException("Database timeout"));

        var handler = new GetSocialActionsQueryHandler(_logger, _socialActionRepository);
        
        // Act
        var result = await handler.Handle(new GetSocialActionsQuery(), CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Value.Should().BeNull();
        result.FailureStatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        
        var error = result.Errors.First();
        error.Code.Should().Be("InternalError");
        error.Message.Should().Be("Database timeout");
    }
}