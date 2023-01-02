using System.Threading;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.Shared;
using Colabora.Application.Shared.Mappers;
using Colabora.Application.UseCases.CreateSocialAction;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Colabora.TestCommons.Fakers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Colabora.UnitTests.Application.Handlers;

public class CreateSocialActionCommandHandlerTests
{
    private readonly ISocialActionRepository _socialActionRepository;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IOrganizationRepository _organizationRepository;

    public CreateSocialActionCommandHandlerTests()
    {
        _socialActionRepository = Substitute.For<ISocialActionRepository>();
        _volunteerRepository = Substitute.For<IVolunteerRepository>();
        _organizationRepository = Substitute.For<IOrganizationRepository>();
    }

    [Fact(DisplayName = "Given a command, when volunteer creator is not found, then it should return an error")]
    public async Task Given_A_Command_When_Volunteer_Creator_Is_Not_Found_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var command = FakeCreateSocialActionCommand.Create();

        _volunteerRepository.GetVolunteerById(command.VolunteerCreatorId).Returns(Volunteer.None);
        var handler = new CreateSocialActionCommandHandler(_volunteerRepository, _organizationRepository, _socialActionRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(ErrorMessages.CreateVolunteerNotFound());
    } 
    
    [Fact(DisplayName = "Given a command, when organization is not found, then it should return an error")]
    public async Task Given_A_Command_When_Organization_Is_Not_Found_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var command = FakeCreateSocialActionCommand.Create();

        _volunteerRepository.GetVolunteerById(command.VolunteerCreatorId).Returns(FakeVolunteer.Create());
        _organizationRepository.GetOrganizationById(command.OrganizationId).Returns(Organization.None);
        
        var handler = new CreateSocialActionCommandHandler(_volunteerRepository, _organizationRepository, _socialActionRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(ErrorMessages.CreateOrganizationNotFound());
    } 
    
    [Fact(DisplayName = "Given a command, when it succeeds then it should return the created social action")]
    public async Task Given_A_Command_When_It_Succeeds_Then_It_Should_Return_The_Created_Social_Action()
    {
        // Arrange
        var command = FakeCreateSocialActionCommand.Create();

        _volunteerRepository.GetVolunteerById(command.VolunteerCreatorId).Returns(FakeVolunteer.Create());
        _organizationRepository.GetOrganizationById(command.OrganizationId).Returns(FakerOrganization.Create());

        var handler = new CreateSocialActionCommandHandler(_volunteerRepository, _organizationRepository, _socialActionRepository);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Error.Should().Be(Error.Empty);
        result.Value.Should().BeEquivalentTo(command.MapToSocialAction(), options => 
            options.Excluding(action => action.CreatedAt));
    } 
}