using System;
using System.Threading;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.Features.RegisterOrganization;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.TestCommons.Fakers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Colabora.UnitTests.Application.Handlers;

public class RegisterOrganizationCommandHandlerTests
{
    private readonly ILogger<RegisterOrganizationCommandHandler> _logger;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IVolunteerRepository _volunteerRepository;
    
    public RegisterOrganizationCommandHandlerTests()
    {
        _logger = Substitute.For<ILogger<RegisterOrganizationCommandHandler>>();
        _organizationRepository = Substitute.For<IOrganizationRepository>();
        _volunteerRepository = Substitute.For<IVolunteerRepository>();
    }
    
    [Fact(DisplayName = "Given a command, when it succeeds, then handler should return the organization created")]
    public async Task Given_A_Command_When_It_Succeeds_Then_Handler_Should_Return_The_Organization_Created()
    {
        // Arrange
        var command = FakeRegisterOrganizationCommand.Create();

        var organization = FakerOrganization.Create(command);

        _volunteerRepository.GetVolunteerById(command.VolunteerCreatorId).Returns(FakeVolunteer.Create());
        
        _organizationRepository.GetOrganization(command.Name, command.Email, command.VolunteerCreatorId).Returns(Task.FromResult(Organization.None));
        _organizationRepository.CreateOrganization(Arg.Any<Organization>()).Returns(organization);

        var handler = new RegisterOrganizationCommandHandler(_logger, _organizationRepository, _volunteerRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Error.Should().Be(Error.Empty);
        result.IsValid.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(organization.MapToRegisterOrganizationResponse());
    }
    
    [Fact(DisplayName = "Given a command, when it fails due conflict, then handler should return an error result")]
    public async Task Given_A_Command_When_It_Fails_Due_Conflict_Then_Handler_Should_Return_An_Error_Result()
    {
        // Arrange
        var command = FakeRegisterOrganizationCommand.Create();

        var organization = FakerOrganization.Create(command);
        
        _volunteerRepository.GetVolunteerById(command.VolunteerCreatorId).Returns(FakeVolunteer.Create());

        _organizationRepository.GetOrganization(command.Name, command.Email, command.VolunteerCreatorId).Returns(organization);
        _organizationRepository.CreateOrganization(Arg.Any<Organization>()).Returns(organization);

        var handler = new RegisterOrganizationCommandHandler(_logger, _organizationRepository, _volunteerRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().BeEquivalentTo(ErrorMessages.CreateOrganizationEmailAlreadyExists(command.Name));
    }
    
    [Fact(DisplayName = "Given a command, when an exception occurs, then handler should return an error result")]
    public async Task Given_A_Command_When_An_Exception_Occurs_Then_Handler_Should_Return_An_Error_Result()
    {
        // Arrange
        var command = FakeRegisterOrganizationCommand.Create();
        
        _volunteerRepository.GetVolunteerById(command.VolunteerCreatorId).Returns(FakeVolunteer.Create());

        _organizationRepository.GetOrganization(command.Name, command.Email, command.VolunteerCreatorId).Returns(Organization.None);
        _organizationRepository.CreateOrganization(Arg.Any<Organization>()).Throws(new TimeoutException("error"));

        var handler = new RegisterOrganizationCommandHandler(_logger, _organizationRepository, _volunteerRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Value.Should().BeNull();
        result.Error.Should().BeEquivalentTo(ErrorMessages.CreateInternalError("error"));
    }
}