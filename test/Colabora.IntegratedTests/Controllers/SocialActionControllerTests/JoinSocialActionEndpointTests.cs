using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Features.CreateSocialAction.Models;
using Colabora.Application.Features.JoinSocialAction.Models;
using Colabora.Application.Features.RegisterOrganization.Models;
using Colabora.Application.Features.RegisterVolunteer.Models;
using Colabora.TestCommons.Fakers;
using FluentAssertions;
using Xunit;
#pragma warning disable CS8602

namespace Colabora.IntegrationTests.Controllers.SocialActionControllerTests;

public partial class SocialActionControllerTests
{
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_Social_Action_Not_Exits_Then_It_Should_Return_An_Error()
    {
        // Arrange
        

        
        
        // Act
        
        
        // Assert
    }
    
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_Volunteer_Not_Exits_Then_It_Should_Return_An_Error()
    {
        // Arrange
        

        // Act
        
        
        // Assert
    }
    
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_Succeeds_Then_It_Should_Return_Success_Status_Code()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        var organizationResponse = await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        var organization = await organizationResponse.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();
        
        var createSocialActionCommand = FakeCreateSocialActionCommand.Create(organization.OrganizationId, volunteer.VolunteerId);
        var createSocialActionResponse = await client.PostAsJsonAsync("api/v1.0/actions", createSocialActionCommand);
        var socialAction = await createSocialActionResponse.Content.ReadFromJsonAsync<CreateSocialActionResponse>();

        var joinSocialActionCommand = new JoinSocialActionCommand(socialAction.SocialActionId, volunteer.VolunteerId);

        // Act
        var response = await client.PostAsJsonAsync($"api/v1.0/actions/{socialAction.SocialActionId}/join", joinSocialActionCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JoinSocialActionResponse>();
        body.Should().NotBeNull();
        body.SocialActionName.Should().Be(socialAction.Title);
        body.VolunteerName.Should().Be($"{volunteer.FirstName} {volunteer.LastName}");
        body.JoinedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_An_Exception_Occurs_Then_It_Should_Return_An_Error()
    {
        // Arrange
        

        // Act
        
        // Assert
        
    }
}