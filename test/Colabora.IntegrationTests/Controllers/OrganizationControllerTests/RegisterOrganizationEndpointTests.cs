using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.IntegrationTests.Fixtures;
using Colabora.TestCommons.Fakers;
using Colabora.WebAPI;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

#pragma warning disable CS8602

namespace Colabora.IntegrationTests.Controllers.OrganizationControllerTests;

public partial class RegisterOrganizationEndpointTests : 
    IClassFixture<WebApplicationFactory<Program>>, 
    IClassFixture<DatabaseFixture>,
    IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly DatabaseFixture _databaseFixture;
    
    public RegisterOrganizationEndpointTests(WebApplicationFactory<Program> factory, DatabaseFixture databaseFixture)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Test"));
        _databaseFixture = databaseFixture;
    }

    [Fact]
    private async Task Given_A_Register_Organization_Request_When_It_Succeeds_Then_It_Should_Return_The_Created_Organizations()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var registerVolunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);

        var volunteer = await registerVolunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var command = FakeRegisterOrganizationCommand.Create(volunteer.VolunteerId);
        
        // Act
        var result = await client.PostAsJsonAsync("/api/v1.0/organizations", command);
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var registerOrganizationResponse = await result.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();

        registerOrganizationResponse.Should().NotBeNull();
        registerOrganizationResponse.OrganizationId.Should().BeGreaterThan(0);
        registerOrganizationResponse.Name.Should().BeEquivalentTo(command.Name);
        registerOrganizationResponse.State.Should().Be(command.State);
        registerOrganizationResponse.CreatedBy.Should().Be(command.VolunteerCreatorId).And.Be(volunteer.VolunteerId);
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

        var errors = await result.Content.ReadFromJsonAsync<IEnumerable<Error>>();
        var errorResponse = errors!.First();
        
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
        
        var command = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        
        // Act
        await client.PostAsJsonAsync("/api/v1.0/organizations", command);
        var result = await client.PostAsJsonAsync("/api/v1.0/organizations", command);
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var errors = await result.Content.ReadFromJsonAsync<List<Error>>();
        var errorResponse = errors!.First();
        errorResponse.Should().NotBeNull();
        errorResponse.Code.Should().Be("CreateOrganizationConflict");
        errorResponse.Message.Should().Be("Already exist an organization with same name and email created by this user");
    }
    
    public async Task InitializeAsync() => await _databaseFixture.ResetDatabase();

    public async Task DisposeAsync() => await _databaseFixture.ResetDatabase();
    
}