using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Colabora.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Colabora.IntegrationTests.Controllers;

public class VolunteerControllerTests : 
    IClassFixture<TestWebApplicationFactory<Program>>,
    IAsyncLifetime
{
    private readonly HttpClient _client;
    private TestWebApplicationFactory<Program> _factory; 
    
    public VolunteerControllerTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Test()
    {
        // Arrange - Act
        var getRequest = await _client.GetAsync("volunteers");
        
        // Assert
        getRequest.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public async Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await service.Database.MigrateAsync();
    }

    public Task DisposeAsync()
    {
        throw new System.NotImplementedException();
    }
}