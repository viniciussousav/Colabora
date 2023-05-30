using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Shared;
using Colabora.Domain.Volunteer;
using Colabora.Infrastructure.Auth;
using Colabora.TestCommons.Fakers.Commands;
using Colabora.TestCommons.Fakers.Shared;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

#pragma warning disable CS8602

using ErrorMessages = Colabora.Application.Shared.ErrorMessages;

namespace Colabora.IntegrationTests.Controllers.VolunteerControllerTests;

public partial class VolunteerControllerTests
{
    [Fact(DisplayName = "Given a register volunteer request, when its valid, then the created volunteer should be returned")]
    public async Task Given_A_Register_Volunteer_Request_When_Its_Valid_Then_The_Created_Volunteer_Should_Be_Returned()
    {
        // Arrange
        var command = FakeRegisterVolunteerCommand.CreateValid();
        
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var authServiceDescriptor = services.Single(service => service.ServiceType == typeof(IAuthService));
                services.Remove(authServiceDescriptor);
   
                services.AddScoped<IAuthService>(_ =>
                {
                    var authService = Substitute.For<IAuthService>();
                    authService.Authenticate(Arg.Any<AuthProvider>(), Arg.Any<string>()).Returns(FakeAuthResult.Create(command.Email));
                    return authService;
                });
            });
        }).CreateClient();
        
        client.DefaultRequestHeaders.Add("OAuthToken", "HeaderValue");
        
        // Act
        var response = await client.PostAsJsonAsync("/api/v1.0/volunteers", command);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var createVolunteerResponse = await response.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();

        createVolunteerResponse.VolunteerId.Should().BePositive();
        createVolunteerResponse.FirstName.Should().Be(command.FirstName);
        createVolunteerResponse.LastName.Should().Be(command.LastName);
        createVolunteerResponse.Email.Should().Be(command.Email);
        createVolunteerResponse.Gender.Should().Be(command.Gender);
        createVolunteerResponse.Birthdate.Should().Be(command.Birthdate);
        createVolunteerResponse.State.Should().Be(command.State);
        createVolunteerResponse.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        createVolunteerResponse.Interests.Should().BeEquivalentTo(command.Interests);
    }
    
    [Fact(DisplayName = "Given a register volunteer request, when already exists a volunteer with same email, then a conflict error should be returned")]
    public async Task Given_A_Register_Volunteer_Request_When_Already_Exists_A_Volunteer_With_Same_Email_Then_A_Conflict_Error_Should_Be_Returned()
    {
        // Arrange
        var command = FakeRegisterVolunteerCommand.CreateValid();
        var existingVolunteer = FakeRegisterVolunteerCommand.CreateValid(email: command.Email);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var authServiceDescriptor = services.Single(service => service.ServiceType == typeof(IAuthService));
                services.Remove(authServiceDescriptor);
   
                services.AddScoped<IAuthService>(_ =>
                {
                    var authService = Substitute.For<IAuthService>();
                    authService.Authenticate(Arg.Any<AuthProvider>(), "HeaderValue").Returns(FakeAuthResult.Create(command.Email));
                    authService.Authenticate(Arg.Any<AuthProvider>(), "HeaderValue2").Returns(FakeAuthResult.Create(existingVolunteer.Email));

                    return authService;
                });
            });
        }).CreateClient();

        client.DefaultRequestHeaders.Add("OAuthToken", "HeaderValue2");
        await client.PostAsJsonAsync("/api/v1.0/volunteers", existingVolunteer);
        
        // Act
        client.DefaultRequestHeaders.Remove("OAuthToken");
        client.DefaultRequestHeaders.Add("OAuthToken", "HeaderValue");
        var response = await client.PostAsJsonAsync("/api/v1.0/volunteers", command);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var errorResponse = await response.Content.ReadFromJsonAsync<List<Error>>();
        errorResponse.Should().ContainEquivalentOf(ErrorMessages.CreateVolunteerEmailAlreadyExists(command.Email));
    }
    
    [Fact(DisplayName = "Given a register volunteer request, when an exception occurs, then an error should be returned")]
    public async Task Given_A_Register_Volunteer_Request_When_An_Exceptions_Occurs_Then_An_Error_Should_Be_Returned()
    {
        // Arrange
        var command = FakeRegisterVolunteerCommand.CreateValid();
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.First(service => service.ServiceType == typeof(IVolunteerRepository));
                services.Remove(dbContextDescriptor);
                    
                services.AddScoped<IVolunteerRepository>(_ =>
                {
                    var volunteerRepositoryMock = Substitute.For<IVolunteerRepository>();
                    volunteerRepositoryMock.GetVolunteerByEmail(Arg.Any<string>()).Throws(new Exception("Hello"));
                    return volunteerRepositoryMock;
                });
                
                var authServiceDescriptor = services.Single(service => service.ServiceType == typeof(IAuthService));
                services.Remove(authServiceDescriptor);
   
                services.AddScoped<IAuthService>(_ =>
                {
                    var authService = Substitute.For<IAuthService>();
                    authService.Authenticate(Arg.Any<AuthProvider>(), "HeaderValue").Returns(FakeAuthResult.Create(command.Email));
                    return authService;
                });
            });
        }).CreateClient();
        
        client.DefaultRequestHeaders.Add("OAuthToken", "HeaderValue");
        
        // Act
        var response = await client.PostAsJsonAsync("/api/v1.0/volunteers", command);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var errorResponse = await response.Content.ReadFromJsonAsync<List<Error>>();
        errorResponse.Should().ContainEquivalentOf(ErrorMessages.CreateInternalError("Hello"));
    }
}