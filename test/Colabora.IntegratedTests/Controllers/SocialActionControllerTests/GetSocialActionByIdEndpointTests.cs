using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.Features.CreateSocialAction.Models;
using Colabora.Application.Features.GetSocialActionById.Models;
using Colabora.Application.Features.RegisterOrganization.Models;
using Colabora.Application.Features.RegisterVolunteer.Models;
using Colabora.Application.Shared;
using Colabora.Domain.Repositories;
using Colabora.TestCommons.Fakers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;
#pragma warning disable CS8602

namespace Colabora.IntegrationTests.Controllers.SocialActionControllerTests;

public partial class SocialActionControllerTests
{
    [Fact]
    public async Task Given_A_GetSocialActionById_Request_When_Social_Action_Exists_Then_It_Should_Return_The_Existing_Social_Action_With_200_StatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        var organizationResponse = await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        var organization = await organizationResponse.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();
        
        var createSocialActionCommand = FakeCreateSocialActionCommand.Create(volunteerCreatorId: volunteer.VolunteerId, organizationId: organization.OrganizationId);
        var socialActionResponse = await client.PostAsJsonAsync("api/v1.0/actions/", createSocialActionCommand);
        var socialAction = await socialActionResponse.Content.ReadFromJsonAsync<CreateSocialActionResponse>();

        // Act
        var response = await client.GetAsync($"api/v1.0/actions/{socialAction.SocialActionId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var getSocialActionBodyResponse = await response.Content.ReadFromJsonAsync<GetSocialActionByIdResponse>();
        getSocialActionBodyResponse.SocialActionId.Should().Be(socialAction.SocialActionId);
        getSocialActionBodyResponse.OrganizationId.Should().Be(socialAction.OrganizationId);
        getSocialActionBodyResponse.Description.Should().Be(socialAction.Description);
        getSocialActionBodyResponse.Title.Should().Be(socialAction.Title);
        getSocialActionBodyResponse.VolunteerCreatorId.Should().Be(socialAction.VolunteerCreatorId);
        getSocialActionBodyResponse.State.Should().Be(socialAction.State);
        getSocialActionBodyResponse.CreatedAt.Should().Be(socialAction.CreatedAt);
        getSocialActionBodyResponse.OccurrenceDate.Should().Be(socialAction.OccurrenceDate);
        getSocialActionBodyResponse.Interests.Should().BeEquivalentTo(socialAction.Interests);
    }
    
    [Fact]
    public async Task Given_A_GetSocialActionById_Request_When_Social_Action_Not_Exists_Then_It_Should_Return_An_Error_With_404_StatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);

        // Act
        var response = await client.GetAsync($"api/v1.0/actions/{1}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var getSocialActionBodyResponse = await response.Content.ReadFromJsonAsync<Error>();
        getSocialActionBodyResponse.Should().BeEquivalentTo(ErrorMessages.CreateSocialActionNotFound());
    }
    
    [Fact]
    public async Task Given_A_GetSocialActionById_Request_When_An_Exception_Occurs_Then_It_Should_Return_An_Error_With_500_StatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);

        client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.Single(service => service.ServiceType == typeof(ISocialActionRepository));
                services.Remove(dbContextDescriptor);
                
                services.AddScoped<ISocialActionRepository>(_ =>
                {
                    var socialActionRepository = Substitute.For<ISocialActionRepository>();
                    socialActionRepository.GetSocialActionById(Arg.Any<int>(), Arg.Any<CancellationToken>()).Throws(new TaskCanceledException("Hello Exception"));
                    return socialActionRepository;
                });
            });
        }).CreateClient();
        
        // Act
        var response = await client.GetAsync($"api/v1.0/actions/{1}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var getSocialActionBodyResponse = await response.Content.ReadFromJsonAsync<Error>();
        getSocialActionBodyResponse.Should().BeEquivalentTo(ErrorMessages.CreateInternalError("Hello Exception"));
    }
}