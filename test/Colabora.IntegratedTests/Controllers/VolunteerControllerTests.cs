using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Colabora.Application.UseCases.GetVolunteers.Models;
using Colabora.Application.UseCases.RegisterVolunteer.Models;
using Colabora.Infrastructure.Persistence;
using Colabora.IntegrationTests.Database;
using Colabora.TestCommons.Fakers;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Colabora.IntegrationTests.Controllers;

public class VolunteerControllerTests : 
    IClassFixture<WebApplicationFactory<Program>>,
    IClassFixture<DatabaseFixture>,
    IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly DatabaseFixture _databaseFixture;
    
    public VolunteerControllerTests(WebApplicationFactory<Program> factory, DatabaseFixture databaseFixture)
    {
        _factory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Test"));
        _client = _factory.CreateClient();
        _jsonSerializerOptions = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        _databaseFixture = databaseFixture;
    }
    
    [Fact(DisplayName = "Given a get volunteers request, when any volunteer is registered, then it should return an empty array of volunteers")]
    public async Task Given_A_Get_Volunteers_Request_When_Any_Volunteer_Is_Registered_Then_It_Should_Return_An_Empty_Array_Of_Volunteers()
    {
        // Arrange - Act
        var response = await _client.GetAsync("api/v1/volunteers");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var getVolunteersResponse = JsonSerializer.Deserialize<GetVolunteersResponse>(content, _jsonSerializerOptions);
        getVolunteersResponse.Should().NotBeNull();
        getVolunteersResponse.Volunteers.Should().BeEmpty();
    }
    
    [Fact(DisplayName = "Given a get volunteers request, when there is a volunteer registered, then it should return an array with registered volunteer")]
    public async Task Given_A_Get_Volunteers_Request_When_There_Is_A_Volunteer_Registered_Then_It_Should_Return_An_Array_With_Registered_Volunteer()
    {
        // Arrange
        var command = FakeRegisterVolunteerCommand.Create();
        
        await _client.PostAsJsonAsync("api/v1/volunteers", command);
        
        // Act
        var response = await _client.GetAsync("api/v1/volunteers");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var getVolunteersResponse = JsonSerializer.Deserialize<GetVolunteersResponse>(content, _jsonSerializerOptions);
        getVolunteersResponse.Should().NotBeNull();
        getVolunteersResponse.Volunteers.Should().NotBeEmpty();
        getVolunteersResponse.Volunteers.Should().Contain(item => item.Email == command.Email);
    }
    
    [Theory(DisplayName = "Given a get volunteers request, when there are volunteers registered, then it should return an array with registered volunteers")]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Given_A_Get_Volunteers_Request_When_There_Are_Volunteers_Registered_Then_It_Should_Return_An_Array_With_Registered_Volunteers(int volunteersRegisteredCount)
    {
        // Arrange
        var volunteersRegistered = new List<RegisterVolunteerCommand>();

        for (var i = 0; i < volunteersRegisteredCount; i++)
        {
            var command = FakeRegisterVolunteerCommand.Create();
            await _client.PostAsJsonAsync("api/v1/volunteers", command);
            volunteersRegistered.Add(command);
        }
        
        // Act
        var response = await _client.GetAsync("api/v1/volunteers");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var getVolunteersResponse = JsonSerializer.Deserialize<GetVolunteersResponse>(content, _jsonSerializerOptions);
        getVolunteersResponse.Should().NotBeNull();
        getVolunteersResponse.Volunteers.Should().NotBeEmpty();

        volunteersRegistered.Should().AllSatisfy(itemResponse =>
        {
            getVolunteersResponse.Volunteers.Should().Contain(command => command.Email == itemResponse.Email);
        });
    }

    public async Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await _databaseFixture.ClearDatabase(dbContext);
    }

    public Task DisposeAsync() => Task.CompletedTask;
    
}