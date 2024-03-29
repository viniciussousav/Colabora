﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using Colabora.Application.Features.Volunteer.RegisterOrganization.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Organization;
using Colabora.Domain.Shared.Errors;
using Colabora.Infrastructure.Auth;
using Colabora.TestCommons.Fakers.Commands;
using Colabora.TestCommons.Fakers.Shared;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

using ErrorMessages = Colabora.Application.Shared.ErrorMessages;

#pragma warning disable CS8602

namespace Colabora.IntegrationTests.Controllers.OrganizationControllerTests;

public partial class RegisterOrganizationEndpointTests
{
    [Fact]
    public async Task Given_A_Get_Organization_By_Id_Request_When_Organization_Is_Not_Found_Then_It_Should_Return_An_Error_With_404_Status_Code()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act 
        var token  = await _authTokenFixture.GenerateTestJwt("example@email.com");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.GetAsync($"/api/v1.0/organizations/{3}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var error = await response.Content.ReadFromJsonAsync<IEnumerable<Error>>();
        error.Should().ContainEquivalentOf(ErrorMessages.CreateOrganizationNotFound());
    }
    
    [Fact]
    public async Task Given_A_Get_Organization_By_Id_Request_When_Organization_Exists_With_No_SocialActions_Then_It_Should_Return_The_Existing_Organization()
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
                    authService.AuthenticateUser(Arg.Any<AuthProvider>(), Arg.Any<string>()).Returns(FakeAuthResult.Create(registerVolunteerCommand.Email));
                    return authService;
                });
            });
        }).CreateClient();
        
        client.DefaultRequestHeaders.Add("OAuthToken", "HeaderValue");
        var registerVolunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await registerVolunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();

        var token  = await _authTokenFixture.GenerateTestJwt(volunteer.Email);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteer.VolunteerId);
        var registerOrganizationResponse = await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        var organization = await registerOrganizationResponse.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();
        
        // Act 
        var response = await client.GetAsync($"/api/v1.0/organizations/{organization.OrganizationId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getOrganizationByIdResponse = await response.Content.ReadFromJsonAsync<GetOrganizationByIdResponse>();

        getOrganizationByIdResponse.Should().NotBeNull();
        getOrganizationByIdResponse.OrganizationId.Should().Be(organization.OrganizationId);
        getOrganizationByIdResponse.Name.Should().BeEquivalentTo(organization.Name);
        getOrganizationByIdResponse.State.Should().Be(organization.State);
        getOrganizationByIdResponse.VolunteerCreatorId.Should().Be(organization.VolunteerCreatorId).And.Be(volunteer.VolunteerId);
        getOrganizationByIdResponse.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
        getOrganizationByIdResponse.Interests.Should().BeEquivalentTo(organization.Interests);
    }
    
    [Fact]
    public async Task Given_A_Get_Organization_By_Id_Request_When_An_Exception_Occurs_Then_It_Should_Return_An_Error_With_500_Status_Code()
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
                    authService.AuthenticateUser(Arg.Any<AuthProvider>(), Arg.Any<string>()).Returns(FakeAuthResult.Create(registerVolunteerCommand.Email));
                    return authService;
                });
            });
        }).CreateClient();
        
        client.DefaultRequestHeaders.Add("OAuthToken", "HeaderValue");
        var registerVolunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await registerVolunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();

        var token  = await _authTokenFixture.GenerateTestJwt(volunteer.Email);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteer.VolunteerId);
        var registerOrganizationResponse = await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        var organization = await registerOrganizationResponse.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();

        client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var serviceDescriptor =
                    services.First(service => service.ServiceType == typeof(IOrganizationRepository));
                services.Remove(serviceDescriptor);

                services.AddScoped<IOrganizationRepository>(_ =>
                {
                    var mockRepository = Substitute.For<IOrganizationRepository>();
                    mockRepository.GetOrganizationById(Arg.Any<Guid>(), Arg.Any<bool>())
                        .Throws(new TaskCanceledException("Hello Exception"));
                    return mockRepository;
                });
            });
        }).CreateClient();

        // Act 
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.GetAsync($"/api/v1.0/organizations/{organization.OrganizationId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var error = await response.Content.ReadFromJsonAsync<List<Error>>();
        error.Should().ContainEquivalentOf(ErrorMessages.CreateInternalError("Hello Exception"));
    }
}