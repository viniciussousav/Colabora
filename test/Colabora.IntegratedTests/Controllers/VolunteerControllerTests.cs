using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Colabora.IntegrationTests.Controllers;

public class VolunteerControllerTests : 
    IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<Program> _factory;
    
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
}