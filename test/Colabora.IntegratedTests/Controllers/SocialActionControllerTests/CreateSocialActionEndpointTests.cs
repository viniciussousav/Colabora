using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.Shared;
using Colabora.Application.UseCases.CreateSocialAction.Models;
using Colabora.Application.UseCases.RegisterOrganization.Models;
using Colabora.Application.UseCases.RegisterVolunteer.Models;
using Colabora.IntegrationTests.Fixtures;
using Colabora.TestCommons.Fakers;
using Colabora.WebAPI;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Colabora.IntegrationTests.Controllers.SocialActionControllerTests;

public class CreateSocialActionEndpointTests : 
    IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    
    
    public CreateSocialActionEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task Given_A_Command_When_Organization_Not_Exists_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = FakeCreateSocialActionCommand.Create();

        // Act
        var response = await client.PostAsJsonAsync("api/v1.0/actions/", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadFromJsonAsync<Error>();
        errorResponse.Should().BeEquivalentTo(ErrorMessages.CreateOrganizationNotFound());
    }
    
    [Fact]
    public async Task Given_A_Command_When_Volunteer_Creator_Not_Exists_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        var organizationResponse = await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        var organization = await organizationResponse.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();
        
        var createSocialActionCommand = FakeCreateSocialActionCommand.Create(organizationId: organization.OrganizationId);

        // Act
        var response = await client.PostAsJsonAsync("api/v1.0/actions/", createSocialActionCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadFromJsonAsync<Error>();
        errorResponse.Should().BeEquivalentTo(ErrorMessages.CreateVolunteerNotFound());
    }
    
    [Fact]
    public async Task Given_A_Command_When_It_Succeeds_Then_It_Should_Return_The_Created_Social_Action()
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

        // Act
        var response = await client.PostAsJsonAsync("api/v1.0/actions/", createSocialActionCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var socialAction = await response.Content.ReadFromJsonAsync<CreateSocialActionResponse>();
        socialAction.SocialActionId.Should().BeGreaterThan(0);
        socialAction.OrganizationId.Should().Be(organization.OrganizationId);
        socialAction.VolunteerCreatorId.Should().Be(volunteer.VolunteerId);
        socialAction.Title.Should().Be(createSocialActionCommand.Title);
        socialAction.Description.Should().Be(createSocialActionCommand.Description);
        socialAction.Interests.Should().BeEquivalentTo(createSocialActionCommand.Interests);
        socialAction.State.Should().Be(createSocialActionCommand.State);
        socialAction.CreatedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
        socialAction.OccurrenceDate.Should().Be(createSocialActionCommand.OccurrenceDate);
    }

    public async Task InitializeAsync()
    {
        await DatabaseFixture.ResetDatabase();
    }

    public async Task DisposeAsync()
    {
        await DatabaseFixture.ResetDatabase();
    }
}