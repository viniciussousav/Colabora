using System;
using System.Threading;
using System.Threading.Tasks;
using Colabora.Application.Features.Volunteer.RegisterVolunteer;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.TestCommons.Fakers;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Colabora.UnitTests.Application.Handlers;

public class RegisterVolunteerCommandHandlerTests
{
    private readonly ILogger<RegisterVolunteerCommandHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<RegisterVolunteerCommand> _validator;

    public RegisterVolunteerCommandHandlerTests()
    {
        _logger = Substitute.For<ILogger<RegisterVolunteerCommandHandler>>();
        _volunteerRepository = Substitute.For<IVolunteerRepository>();
        _validator = Substitute.For<IValidator<RegisterVolunteerCommand>>();
    }
    
    [Fact(DisplayName = "Given a command, when it succeeds, then it should return the created volunteer")]
    public async Task Given_A_Command_When_It_Succeeds_Then_It_Should_Return_The_Created_Volunteer()
    {
        // Assert
        var command = FakeRegisterVolunteerCommand.Create();
        _volunteerRepository.GetVolunteerByEmail(command.Email).Returns(Volunteer.None);

        var volunteer = FakeVolunteer.Create(command);
        _volunteerRepository.CreateVolunteer(Arg.Is<Volunteer>(v => v.Email == command.Email)).Returns(volunteer);

        _validator.ValidateAsync(command).Returns(new ValidationResult());
        
        var handler = new RegisterVolunteerCommandHandler(_logger, _volunteerRepository, _validator);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(volunteer.MapToRegisterVolunteerResponse());
        result.Errors.Should().BeEmpty();
    }
    
    [Fact(DisplayName = "Given a command, when exists a volunteer registered with same email, the it should return error")]
    public async Task Given_A_Command_When_Exists_A_Volunteer_Registered_With_Same_Email_Then_It_Should_Return_Error()
    {
        // Assert
        var command = FakeRegisterVolunteerCommand.Create();
        
        _validator.ValidateAsync(command).Returns(new ValidationResult());

        var volunteer = FakeVolunteer.Create();
        _volunteerRepository.GetVolunteerByEmail(command.Email).Returns(volunteer);
        
        var handler = new RegisterVolunteerCommandHandler(_logger, _volunteerRepository, _validator);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should()
            .HaveCount(1)
            .And
            .ContainEquivalentOf(ErrorMessages.CreateVolunteerEmailAlreadyExists(command.Email));
    }
    
    [Fact(DisplayName = "Given a command, when an exception occurs, the it should return error")]
    public async Task Given_A_Command_When_An_Exception_Occurs_Then_It_Should_Return_Error()
    {
        // Assert
        var command = FakeRegisterVolunteerCommand.Create();
        _volunteerRepository.GetVolunteerByEmail(command.Email).Throws(new Exception("Hello Exception"));
        
        _validator.ValidateAsync(command).Returns(new ValidationResult());

        var handler = new RegisterVolunteerCommandHandler(_logger, _volunteerRepository, _validator);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Errors.Should()
            .HaveCount(1)
            .And
            .ContainEquivalentOf(ErrorMessages.CreateInternalError("Hello Exception"));
    }
}