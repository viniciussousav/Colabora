using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Repositories;
using Colabora.Infrastructure.Auth;
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

#pragma warning disable CS8602

namespace Colabora.IntegrationTests.Controllers.VolunteerControllerTests;

public partial class VolunteerControllerTests : 
    IClassFixture<WebApplicationFactory<Program>>,
    IClassFixture<DatabaseFixture>,
    IClassFixture<AuthTokenFixture>,
    IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly DatabaseFixture _databaseFixture;
    private readonly AuthTokenFixture _authTokenFixture;

    public VolunteerControllerTests(WebApplicationFactory<Program> factory, DatabaseFixture databaseFixture, AuthTokenFixture authTokenFixture)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Test"));
        _databaseFixture = databaseFixture;
        _authTokenFixture = authTokenFixture;
    }
    
    [Fact(DisplayName = "Given a get volunteers request, when any volunteer is registered, then it should return an empty array of volunteers")]
    public async Task Given_A_Get_Volunteers_Request_When_Any_Volunteer_Is_Registered_Then_It_Should_Return_An_Empty_Array_Of_Volunteers()
    {
        // Arrange 
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("api/v1/volunteers");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getVolunteersResponse = await response.Content.ReadFromJsonAsync<GetVolunteersResponse>();
        
        getVolunteersResponse.Should().NotBeNull();
        getVolunteersResponse.Volunteers.Should().BeEmpty();
    }
    
    [Fact(DisplayName = "Given a get volunteers request, when there is a volunteer registered, then it should return an array with registered volunteer")]
    public async Task Given_A_Get_Volunteers_Request_When_There_Is_A_Volunteer_Registered_Then_It_Should_Return_An_Array_With_Registered_Volunteer()
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
        await client.PostAsJsonAsync("api/v1/volunteers", registerVolunteerCommand);

        // Act
        var response = await client.GetAsync("api/v1/volunteers");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getVolunteersResponse = await  response.Content.ReadFromJsonAsync<GetVolunteersResponse>();
        
        getVolunteersResponse.Should().NotBeNull();
        getVolunteersResponse.Volunteers.Should().NotBeEmpty();

        var getVolunteersItemResponse = getVolunteersResponse.Volunteers.First();
        getVolunteersItemResponse.VolunteerId.Should().BePositive();
        getVolunteersItemResponse.Email.Should().Be(registerVolunteerCommand.Email);
        getVolunteersItemResponse.FirstName.Should().Be(registerVolunteerCommand.FirstName);
        getVolunteersItemResponse.LastName.Should().Be(registerVolunteerCommand.LastName);
        getVolunteersItemResponse.State.Should().Be(registerVolunteerCommand.State);
        getVolunteersItemResponse.Interests.Should().BeEquivalentTo(registerVolunteerCommand.Interests);
        getVolunteersItemResponse.Birthdate.Should().Be(registerVolunteerCommand.Birthdate);
        getVolunteersItemResponse.Gender.Should().Be(registerVolunteerCommand.Gender);
        getVolunteersItemResponse.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }
    
    [Theory(DisplayName = "Given a get volunteers request, when there are volunteers registered, then it should return an array with registered volunteers")]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Given_A_Get_Volunteers_Request_When_There_Are_Volunteers_Registered_Then_It_Should_Return_An_Array_With_Registered_Volunteers(int volunteersRegisteredCount)
    {
        // Arrange
        var volunteersRegistered = new List<RegisterVolunteerCommand>();
        var authService = Substitute.For<IAuthService>();
        for (var i = 0; i < volunteersRegisteredCount; i++)
        {
            var command = FakeRegisterVolunteerCommand.CreateValid();
            authService.Authenticate(Arg.Any<AuthProvider>(), $"HeaderValue{i}").Returns(FakeAuthResult.Create(command.Email));
            volunteersRegistered.Add(command);
        }
        
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var authServiceDescriptor = services.Single(service => service.ServiceType == typeof(IAuthService));
                services.Remove(authServiceDescriptor);
                services.AddScoped<IAuthService>(_ => authService);
            });
        }).CreateClient();

        for(var i = 0; i < volunteersRegisteredCount; i++)
        {
            client.DefaultRequestHeaders.Remove("OAuthToken");
            client.DefaultRequestHeaders.Add("OAuthToken", $"HeaderValue{i}");
            await client.PostAsJsonAsync("api/v1/volunteers", volunteersRegistered[i]);
        }
        
        // Act
        var response = await client.GetAsync("api/v1/volunteers");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getVolunteersResponse = await response.Content.ReadFromJsonAsync<GetVolunteersResponse>();
        getVolunteersResponse.Should().NotBeNull();
        getVolunteersResponse.Volunteers.Should().NotBeEmpty();

        volunteersRegistered.Should().AllSatisfy(itemResponse =>
            getVolunteersResponse.Volunteers.Should().Contain(item => item.Email == itemResponse.Email));
    }

    [Fact(DisplayName = "Given a get volunteers request, when an exception occurs, then it should return an internal server error")]
    public async Task Given_A_Get_Volunteers_Request_When_An_Exceptions_Occurs_Then_It_Should_Return_An_Internal_Server_Error()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.Single(service => service.ServiceType == typeof(IVolunteerRepository));
                services.Remove(dbContextDescriptor);
                
                services.AddScoped<IVolunteerRepository>(_ =>
                {
                    var volunteerRepositoryMock = Substitute.For<IVolunteerRepository>();
                    volunteerRepositoryMock.GetAllVolunteers().Throws(new Exception("Hello Exception"));
                    return volunteerRepositoryMock;
                });
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/volunteers");
        
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