using System;
using System.Threading;
using System.Threading.Tasks;
using Colabora.Application.Handlers.Volunteers;
using Colabora.Application.Handlers.Volunteers.RegisterVolunteer;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.TestCommons.Fakers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Colabora.UnitTests.Application.Handlers;

public class RegisterVolunteerCommandHandlerTests
{
    private readonly ILogger<RegisterVolunteerCommandHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;

    public RegisterVolunteerCommandHandlerTests()
    {
        _logger = Substitute.For<ILogger<RegisterVolunteerCommandHandler>>();
        _volunteerRepository = Substitute.For<IVolunteerRepository>();
    }
    
    [Fact(DisplayName = "Given a command, when it succeeds, then it should return the created volunteer")]
    public async Task Given_A_Command_When_It_Succeeds_Then_It_Should_Return_The_Created_Volunteer()
    {
        // Assert
        var command = FakeRegisterVolunteerCommand.Create();
        _volunteerRepository.GetVolunteerByEmail(command.Email).Returns(Volunteer.Empty);

        var volunteer = FakeVolunteer.Create(command);
        _volunteerRepository.CreateVolunteer(Arg.Is<Volunteer>(v => v.Email == command.Email)).Returns(volunteer);

        var handler = new RegisterVolunteerCommandHandler(_logger, _volunteerRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Skip.Should().BeFalse();
        result.IsValid.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(volunteer.MapToRegisterVolunteerResponse());
        result.Errors.Should().BeEmpty();
    }
    
    [Fact(DisplayName = "Given a command, when exists a volunteer registered with same email, the it should return error")]
    public async Task Given_A_Command_When_Exists_A_Volunteer_Registered_With_Same_Email_Then_It_Should_Return_Error()
    {
        // Assert
        var command = FakeRegisterVolunteerCommand.Create();
        var volunteer = FakeVolunteer.Create();
        _volunteerRepository.GetVolunteerByEmail(command.Email).Returns(volunteer);
        
        var handler = new RegisterVolunteerCommandHandler(_logger, _volunteerRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Skip.Should().BeFalse();
        result.IsValid.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().ContainEquivalentOf(ErrorMessages.CreateEmailConflictErrorMessage(command.Email));
    }
    
    [Fact(DisplayName = "Given a command, when an exception occurs, the it should return error")]
    public async Task Given_A_Command_When_An_Exception_Occurs_Then_It_Should_Return_Error()
    {
        // Assert
        var command = FakeRegisterVolunteerCommand.Create();
        _volunteerRepository.GetVolunteerByEmail(command.Email).Throws(new Exception("Hello Exception"));
        
        var handler = new RegisterVolunteerCommandHandler(_logger, _volunteerRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Skip.Should().BeFalse();
        result.IsValid.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should().ContainEquivalentOf(ErrorMessages.CreateInternalErrorException("Hello Exception"));
    }
}