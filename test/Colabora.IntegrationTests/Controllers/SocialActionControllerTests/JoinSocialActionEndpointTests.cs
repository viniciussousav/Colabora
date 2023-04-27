using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Features.SocialAction.JoinSocialAction.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Application.Shared;
using Colabora.Infrastructure.Auth;
using Colabora.TestCommons.Fakers.Commands;
using Colabora.TestCommons.Fakers.Shared;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

#pragma warning disable CS8602

namespace Colabora.IntegrationTests.Controllers.SocialActionControllerTests;

public partial class SocialActionControllerTests
{
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_Social_Action_Not_Exits_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var registerVolunteerCommand = FakeRegisterVolunteerCommand.CreateValid();
        
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var authServiceDescriptor = services.Single(service => service.ServiceType == typeof(IAuthService));
                services.Remove(authServiceDescriptor);
   
                services.AddScoped<IAuthService>(_ =>
                {
                    var authService = Substitute.For<IAuthService>();
                    authService.Authenticate(Arg.Any<AuthProvider>(), Arg.Any<string>()).Returns(FakeAuthResult.Create(registerVolunteerCommand.Email));
                    return authService;
                });
            });
        }).CreateClient();
        
        client.DefaultRequestHeaders.Add("OAuthToken", "HeaderValue");
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var token  = await _authTokenFixture.GenerateTestJwt(volunteer.Email);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        
        var joinSocialActionCommand = new JoinSocialActionCommand(123413, volunteer.VolunteerId);

        // Act
        var response = await client.PostAsJsonAsync($"api/v1.0/actions/{123413}/join", joinSocialActionCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var body = await response.Content.ReadFromJsonAsync<List<Error>>();
        body!.First().Should().BeEquivalentTo(ErrorMessages.CreateSocialActionNotFound());
    }
    
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_Volunteer_Not_Exits_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var registerVolunteerCommand = FakeRegisterVolunteerCommand.CreateValid();
        
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var authServiceDescriptor = services.Single(service => service.ServiceType == typeof(IAuthService));
                services.Remove(authServiceDescriptor);
   
                services.AddScoped<IAuthService>(_ =>
                {
                    var authService = Substitute.For<IAuthService>();
                    authService.Authenticate(Arg.Any<AuthProvider>(), Arg.Any<string>()).Returns(FakeAuthResult.Create(registerVolunteerCommand.Email));
                    return authService;
                });
            });
        }).CreateClient();
        
        client.DefaultRequestHeaders.Add("OAuthToken", "HeaderValue");
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var token  = await _authTokenFixture.GenerateTestJwt(volunteer.Email);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        var organizationResponse = await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        var organization = await organizationResponse.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();
        
        var createSocialActionCommand = FakeCreateSocialActionCommand.Create(organization.OrganizationId, volunteer.VolunteerId);
        var createSocialActionResponse = await client.PostAsJsonAsync("api/v1.0/actions", createSocialActionCommand);
        var socialAction = await createSocialActionResponse.Content.ReadFromJsonAsync<CreateSocialActionResponse>();

        var invalidVolunteer = 123145;
        var joinSocialActionCommand = new JoinSocialActionCommand(socialAction.SocialActionId, invalidVolunteer);

        // Act
        var response = await client.PostAsJsonAsync($"api/v1.0/actions/{socialAction.SocialActionId}/join", joinSocialActionCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var body = await response.Content.ReadFromJsonAsync<List<Error>>();
        body.Should().NotBeNull();
        body!.First().Should().BeEquivalentTo(ErrorMessages.CreateVolunteerNotFound());
    }
    
    [Fact]
    public async Task Given_A_Join_Social_Action_Command_When_Succeeds_Then_It_Should_Return_Success_Status_Code()
    {
        // Arrange
        var registerVolunteerCommand = FakeRegisterVolunteerCommand.CreateValid();
        
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var authServiceDescriptor = services.Single(service => service.ServiceType == typeof(IAuthService));
                services.Remove(authServiceDescriptor);
   
                services.AddScoped<IAuthService>(_ =>
                {
                    var authService = Substitute.For<IAuthService>();
                    authService.Authenticate(Arg.Any<AuthProvider>(), Arg.Any<string>()).Returns(FakeAuthResult.Create(registerVolunteerCommand.Email));
                    return authService;
                });
            });
        }).CreateClient();
        
        client.DefaultRequestHeaders.Add("OAuthToken", "HeaderValue");
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var token  = await _authTokenFixture.GenerateTestJwt(volunteer.Email);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
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
}