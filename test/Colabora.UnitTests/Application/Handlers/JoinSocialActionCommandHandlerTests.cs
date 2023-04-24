using System.Threading;
using System.Threading.Tasks;
using Colabora.Application.Features.SocialAction.JoinSocialAction;
using Colabora.Application.Features.SocialAction.JoinSocialAction.Models;
using Colabora.Application.Shared;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.TestCommons.Fakers.Domain;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Colabora.UnitTests.Application.Handlers;

public class JoinSocialActionCommandHandlerTests
{
    private readonly ILogger<JoinSocialActionCommandHandler> _logger;
    private readonly ISocialActionRepository _socialActionRepository;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<JoinSocialActionCommand> _validator;

    public JoinSocialActionCommandHandlerTests()
    {
        _logger = Substitute.For<ILogger<JoinSocialActionCommandHandler>>();
        _socialActionRepository = Substitute.For<ISocialActionRepository>();
        _volunteerRepository = Substitute.For<IVolunteerRepository>();
        _validator = Substitute.For<IValidator<JoinSocialActionCommand>>();
    }
    
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_Social_Action_Not_Exits_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var command = new JoinSocialActionCommand(1, 2);
        _socialActionRepository.GetSocialActionById(command.SocialActionId, CancellationToken.None).Returns(SocialAction.None);

        _validator.ValidateAsync(command).Returns(new ValidationResult());
        
        var handler = new JoinSocialActionCommandHandler(_logger, _socialActionRepository, _volunteerRepository, _validator);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainEquivalentOf(ErrorMessages.CreateSocialActionNotFound());
    }
    
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_Volunteer_Not_Exits_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var socialAction = FakeSocialAction.Create();
        var command = new JoinSocialActionCommand(socialAction.SocialActionId, 2);
        
        _validator.ValidateAsync(command).Returns(new ValidationResult());

        _socialActionRepository.GetSocialActionById(socialAction.SocialActionId, CancellationToken.None).Returns(socialAction);
        _volunteerRepository.GetVolunteerById(command.VolunteerId).Returns(Volunteer.None);
        
        var handler = new JoinSocialActionCommandHandler(_logger, _socialActionRepository, _volunteerRepository, _validator);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainEquivalentOf(ErrorMessages.CreateVolunteerNotFound());
    }
    
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_Succeeds_Then_It_Should_A_Valid_Result()
    {
        // Arrange
        var socialAction = FakeSocialAction.Create();
        var volunteer = FakeVolunteer.Create();
        var command = new JoinSocialActionCommand(socialAction.SocialActionId, volunteer.VolunteerId);

        _validator.ValidateAsync(command).Returns(new ValidationResult());

        _socialActionRepository.GetSocialActionById(socialAction.SocialActionId, CancellationToken.None).Returns(socialAction);
        _volunteerRepository.GetVolunteerById(command.VolunteerId).Returns(volunteer);
        
        var handler = new JoinSocialActionCommandHandler(_logger, _socialActionRepository, _volunteerRepository, _validator);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_An_Exception_Occurs_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var socialAction = FakeSocialAction.Create();
        var volunteer = FakeVolunteer.Create();
        var command = new JoinSocialActionCommand(socialAction.SocialActionId, volunteer.VolunteerId);
        
        _validator.ValidateAsync(command).Returns(new ValidationResult());

        _socialActionRepository.GetSocialActionById(socialAction.SocialActionId, CancellationToken.None).Returns(socialAction);
        _volunteerRepository.GetVolunteerById(command.VolunteerId).Returns(volunteer);
        _socialActionRepository.CreateParticipation(socialAction.SocialActionId, Arg.Any<Participation>()).Throws(new TaskCanceledException("Timeout"));
        
        var handler = new JoinSocialActionCommandHandler(_logger, _socialActionRepository, _volunteerRepository, _validator);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .HaveCount(1)
            .And
            .ContainEquivalentOf(ErrorMessages.CreateInternalError("Timeout"));
    }
}