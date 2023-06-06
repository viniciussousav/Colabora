using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Shared;
using Colabora.Domain.SocialAction;
using Colabora.Infrastructure.Auth;
using Colabora.Infrastructure.Messaging.Producer;
using Colabora.IntegrationTests.Fixtures;
using Colabora.TestCommons.Fakers.Commands;
using Colabora.TestCommons.Fakers.Shared;
using Colabora.WebAPI;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

using ErrorMessages = Colabora.Application.Shared.ErrorMessages;

#pragma warning disable CS8602

namespace Colabora.IntegrationTests.Controllers.SocialActionControllerTests;

public partial class SocialActionControllerTests : 
    IClassFixture<WebApplicationFactory<Program>>, 
    IClassFixture<DatabaseFixture>, 
    IClassFixture<AuthTokenFixture>,
    IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly DatabaseFixture _databaseFixture;
    private readonly AuthTokenFixture _authTokenFixture;

    public SocialActionControllerTests(WebApplicationFactory<Program> factory, DatabaseFixture databaseFixture, AuthTokenFixture authTokenFixture)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var messageProducerDescriptor = services.Single(service => service.ServiceType == typeof(IMessageProducer));
                services.Remove(messageProducerDescriptor);
                services.AddTransient<IMessageProducer>(_ => Substitute.For<IMessageProducer>());
            });
            builder.UseEnvironment("Test");
        });
        
        _databaseFixture = databaseFixture;
        _authTokenFixture = authTokenFixture;
    }
    
    [Fact]
    public async Task Given_A_Command_When_Organization_Not_Exists_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = FakeCreateSocialActionCommand.Create();

        // Act
        var token  = await _authTokenFixture.GenerateTestJwt("test@email.com");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.PostAsJsonAsync("api/v1.0/actions/", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errors = await response.Content.ReadFromJsonAsync<List<Error>>();
        var errorResponse = errors!.First();
        errorResponse.Should().BeEquivalentTo(ErrorMessages.CreateOrganizationNotFound());
    }
    
    [Fact]
    public async Task Given_A_Command_When_Volunteer_Creator_Not_Exists_Then_It_Should_Return_An_Error()
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
        
        var createSocialActionCommand = FakeCreateSocialActionCommand.Create(organizationId: organization.OrganizationId);

        // Act
        var response = await client.PostAsJsonAsync("api/v1.0/actions/", createSocialActionCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorResponse = await response.Content.ReadFromJsonAsync<List<Error>>();
        errorResponse.Should().ContainEquivalentOf(ErrorMessages.CreateVolunteerNotFound());
    }
    
    [Fact]
    public async Task Given_A_Command_When_It_Succeeds_Then_It_Should_Return_The_Created_Social_Action()
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

    [Fact]
    public async Task Given_A_Command_When_An_Exception_Occurs_Then_It_Should_Return_An_Error_Response()
    {
        // Arrange
        var registerVolunteerCommand = FakeRegisterVolunteerCommand.CreateValid();

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var socialActionRepositoryServiceDescriptor = services.Single(service => service.ServiceType == typeof(ISocialActionRepository));
                services.Remove(socialActionRepositoryServiceDescriptor);
                
                var authServiceDescriptor = services.Single(service => service.ServiceType == typeof(IAuthService));
                services.Remove(authServiceDescriptor);
                
                services.AddScoped<ISocialActionRepository>(_ =>
                {
                    var socialActionRepository = Substitute.For<ISocialActionRepository>();
                    socialActionRepository.CreateSocialAction(Arg.Any<SocialAction>()).Throws(new TaskCanceledException("Hello Exception"));
                    return socialActionRepository;
                });
                
                services.AddScoped<IAuthService>(_ =>
                {
                    var authService = Substitute.For<IAuthService>();
                    authService.Authenticate(Arg.Any<AuthProvider>(), Arg.Any<string>()).Returns(FakeAuthResult.Create(registerVolunteerCommand.Email));
                    return authService;
                });
            });
        }).CreateClient();

        client.DefaultRequestHeaders.Add("OAuthToken", "OAuthTokenValue");
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var token  = await _authTokenFixture.GenerateTestJwt(volunteer.Email);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        var organizationResponse = await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        var organization = await organizationResponse.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();
        
        var createSocialActionCommand = FakeCreateSocialActionCommand.Create(organization.OrganizationId, volunteer.VolunteerId);

        // Act
        var response = await client.PostAsJsonAsync("api/v1.0/actions/", createSocialActionCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var errorResponse = await response.Content.ReadFromJsonAsync<List<Error>>();
        
        var error = errorResponse!.First();
        error.Should().NotBeNull();
        error.Code.Should().Be("InternalError");
        error.Message.Should().Be("Hello Exception");
    }
    
    public async Task InitializeAsync() => await _databaseFixture.ResetDatabase();

    public async Task DisposeAsync() => await _databaseFixture.ResetDatabase();
    
}