using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.UseCases.RegisterOrganization.Models;
using Colabora.Application.UseCases.RegisterVolunteer.Models;
using Colabora.IntegrationTests.Fixtures;
using Colabora.TestCommons.Fakers;
using Colabora.WebAPI;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Colabora.IntegrationTests.Controllers.OrganizationControllerTests;

public class RegisterOrganizationEndpointTests : 
    IClassFixture<WebApplicationFactory<Program>>, 
    IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;

    public RegisterOrganizationEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    private async Task Given_A_Register_Organization_Request_When_It_Succeeds_Then_It_Should_Return_The_Created_Organizations()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var registerVolunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);

        var volunteer = await registerVolunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var command = FakeRegisterOrganizationCommand.Create(volunteer?.Id);
        
        // Act
        var result = await client.PostAsJsonAsync("/api/v1.0/organizations", command);
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var registerOrganizationResponse = await result.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();

        registerOrganizationResponse.Should().NotBeNull();
        registerOrganizationResponse.Id.Should().BeGreaterThan(0);
        registerOrganizationResponse.Name.Should().BeEquivalentTo(command.Name);
        registerOrganizationResponse.State.Should().Be(command.State);
        registerOrganizationResponse.CreatedBy.Should().Be(command.VolunteerCreatorId).And.Be(volunteer.Id);
        registerOrganizationResponse.CreatedAt.AddHours(-3).Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        registerOrganizationResponse.Interests.Should().BeEquivalentTo(command.Interests);
    }
    
    [Fact]
    private async Task Given_A_Register_Organization_Request_When_Volunteer_Creator_Is_Not_Found_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = FakeRegisterOrganizationCommand.Create();
        
        // Act
        var result = await client.PostAsJsonAsync("/api/v1.0/organizations", command);
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await result.Content.ReadFromJsonAsync<Error>();
        errorResponse.Should().NotBeNull();
        errorResponse.Code.Should().Be("VolunteerNotFound");
        errorResponse.Message.Should().Be("Volunteer not found");
    }
    
    [Fact]
    private async Task Given_A_Register_Organization_Request_When_Organization_Is_Already_Registered_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var registerVolunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await registerVolunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var command = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.Id);
        
        // Act
        await client.PostAsJsonAsync("/api/v1.0/organizations", command);
        var result = await client.PostAsJsonAsync("/api/v1.0/organizations", command);
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var errorResponse = await result.Content.ReadFromJsonAsync<Error>();
        errorResponse.Should().NotBeNull();
        errorResponse.Code.Should().Be("CreateOrganizationConflict");
        errorResponse.Message.Should().Be($"Already exist an organization with same name and email created by this user");
    }


    public async Task InitializeAsync()
    {
        await DatabaseFixture.ApplyMigration();
        await DatabaseFixture.ClearDatabase();
    }

    public async Task DisposeAsync()
    {
        await DatabaseFixture.ClearDatabase();
    }
}