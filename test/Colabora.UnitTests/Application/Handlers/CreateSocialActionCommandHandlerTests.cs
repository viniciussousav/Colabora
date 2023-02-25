using System.Threading;
using System.Threading.Tasks;
using Colabora.Application.Features.CreateSocialAction;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.TestCommons.Fakers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Colabora.UnitTests.Application.Handlers;

public class CreateSocialActionCommandHandlerTests
{
    private readonly ILogger<CreateSocialActionCommandHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ISocialActionRepository _socialActionRepository;
    
    public CreateSocialActionCommandHandlerTests()
    {
        _logger = Substitute.For<ILogger<CreateSocialActionCommandHandler>>();
        _volunteerRepository = Substitute.For<IVolunteerRepository>();
        _organizationRepository = Substitute.For<IOrganizationRepository>();
        _socialActionRepository = Substitute.For<ISocialActionRepository>();
    }

    [Fact]
    public async Task Given_A_Command_When_Organization_Not_Exists_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var command = FakeCreateSocialActionCommand.Create();
        
        _organizationRepository.GetOrganizationById(command.OrganizationId).Returns(Organization.None);

        var handler = new CreateSocialActionCommandHandler(_logger, _volunteerRepository, _organizationRepository,_socialActionRepository);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(ErrorMessages.OrganizationNotFound));
        result.Error.Message.Should().Be("Organization not found");
    }
    
    [Fact]
    public async Task Given_A_Command_When_Volunteer_Creator_Not_Exists_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var command = FakeCreateSocialActionCommand.Create();
        
        _organizationRepository.GetOrganizationById(command.OrganizationId).Returns(FakerOrganization.Create());
        _volunteerRepository.GetVolunteerById(command.VolunteerCreatorId).Returns(Volunteer.None);

        var handler = new CreateSocialActionCommandHandler(_logger, _volunteerRepository, _organizationRepository,_socialActionRepository);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(ErrorMessages.VolunteerNotFound));
        result.Error.Message.Should().Be("Volunteer not found");
    }
    
    [Fact]
    public async Task Given_A_Command_When_It_Succeeds_Then_It_Should_Return_The_Created_Social_Action()
    {
        // Arrange
        var command = FakeCreateSocialActionCommand.Create();
        
        _organizationRepository.GetOrganizationById(command.OrganizationId).Returns(FakerOrganization.Create());
        _volunteerRepository.GetVolunteerById(command.VolunteerCreatorId).Returns(FakeVolunteer.Create());

        var createdSocialAction = command.MapToSocialAction();
        _socialActionRepository.CreateSocialAction(Arg.Is<SocialAction>(action => action.Title == command.Title)).Returns(createdSocialAction);
        
        var handler = new CreateSocialActionCommandHandler(_logger, _volunteerRepository, _organizationRepository,_socialActionRepository);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}