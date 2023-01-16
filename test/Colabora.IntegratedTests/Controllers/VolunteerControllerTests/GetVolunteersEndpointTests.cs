using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.UseCases.GetVolunteers.Models;
using Colabora.Application.UseCases.RegisterVolunteer.Models;
using Colabora.Domain.Repositories;
using Colabora.IntegrationTests.Fixtures;
using Colabora.TestCommons.Fakers;
using Colabora.WebAPI;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Colabora.IntegrationTests.Controllers.VolunteerControllerTests;

public partial class VolunteerControllerTests : 
    IClassFixture<WebApplicationFactory<Program>>,
    IClassFixture<HelperFixture>,
    IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;

    public VolunteerControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Test"));
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
        var client = _factory.CreateClient();

        var command = FakeRegisterVolunteerCommand.Create();
        await client.PostAsJsonAsync("api/v1/volunteers", command);

        // Act
        var response = await client.GetAsync("api/v1/volunteers");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getVolunteersResponse = await  response.Content.ReadFromJsonAsync<GetVolunteersResponse>();
        
        getVolunteersResponse.Should().NotBeNull();
        getVolunteersResponse.Volunteers.Should().NotBeEmpty();

        var registeredVolunteer = getVolunteersResponse.Volunteers.First();
        registeredVolunteer.Id.Should().BePositive();
        registeredVolunteer.Email.Should().Be(command.Email);
        registeredVolunteer.FirstName.Should().Be(command.FirstName);
        registeredVolunteer.LastName.Should().Be(command.LastName);
        registeredVolunteer.State.Should().Be(command.State);
        registeredVolunteer.Interests.Should().BeEquivalentTo(command.Interests);
        registeredVolunteer.Birthdate.Should().Be(command.Birthdate);
        registeredVolunteer.Gender.Should().Be(command.Gender);
        registeredVolunteer.CreatedAt.AddHours(-3).Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }
    
    [Theory(DisplayName = "Given a get volunteers request, when there are volunteers registered, then it should return an array with registered volunteers")]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Given_A_Get_Volunteers_Request_When_There_Are_Volunteers_Registered_Then_It_Should_Return_An_Array_With_Registered_Volunteers(int volunteersRegisteredCount)
    {
        // Arrange
        var client = _factory.CreateClient();
        
        var volunteersRegistered = new List<RegisterVolunteerCommand>();
        for (var i = 0; i < volunteersRegisteredCount; i++)
        {
            var command = FakeRegisterVolunteerCommand.Create();
            await client.PostAsJsonAsync("api/v1/volunteers", command);
            volunteersRegistered.Add(command);
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

        var errorResponse = await response.Content.ReadFromJsonAsync<Error>();
        errorResponse.Should().NotBeNull();
        errorResponse.Code.Should().Be("InternalError");
        errorResponse.Message.Should().Be("Hello Exception");
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