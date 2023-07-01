using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Features.SocialAction.GetSocialActions.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Shared;
using Colabora.Domain.SocialAction;
using Colabora.Infrastructure.Auth;
using Colabora.TestCommons.Fakers.Commands;
using Colabora.TestCommons.Fakers.Shared;
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
    public async Task Given_A_Get_Social_Actions_Request_When_Exists_A_Social_Action_Then_It_Should_Be_Returned_With_Status_Code_200()
    {
        // Arrange
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
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var token  = await _authTokenFixture.GenerateTestJwt(volunteer.Email);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        var organizationResponse = await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        var organization = await organizationResponse.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();
        
        var createSocialActionCommand = FakeCreateSocialActionCommand.Create(organization.OrganizationId, volunteer.VolunteerId);
        await client.PostAsJsonAsync("api/v1.0/actions/", createSocialActionCommand);

        // Act
        var response = await client.GetAsync("/api/v1.0/actions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var getSocialActionsResponse = await response.Content.ReadFromJsonAsync<GetSocialActionsResponse>();
        getSocialActionsResponse.Should().NotBeNull();
        getSocialActionsResponse.SocialActions.Count.Should().Be(1);

        var getSocialActionItem = getSocialActionsResponse.SocialActions.First();
        getSocialActionItem.SocialActionId.Should().BeGreaterThan(0);
        getSocialActionItem.OrganizationId.Should().Be(createSocialActionCommand.OrganizationId);
        getSocialActionItem.VolunteerCreatorId.Should().Be(createSocialActionCommand.VolunteerCreatorId);
        getSocialActionItem.Title.Should().Be(createSocialActionCommand.Title);
        getSocialActionItem.Description.Should().Be(createSocialActionCommand.Description);
        getSocialActionItem.Interests.Should().BeEquivalentTo(createSocialActionCommand.Interests);
        getSocialActionItem.State.Should().Be(createSocialActionCommand.State);
        getSocialActionItem.CreatedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
        getSocialActionItem.OccurrenceDate.Should().BeCloseTo(createSocialActionCommand.OccurrenceDate, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task Given_A_Get_Social_Actions_Request_When_No_Social_Action_Exists_Then_It_Should_Return_An_Empty_List_With_Status_Code_200()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/api/v1.0/actions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var getSocialActionsResponse = await response.Content.ReadFromJsonAsync<GetSocialActionsResponse>();
        getSocialActionsResponse.Should().NotBeNull();
        getSocialActionsResponse.SocialActions.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Given_A_Get_Social_Actions_Request_When_An_Exception_Occurs_Then_It_Should_Return_An_Error()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.Single(service => service.ServiceType == typeof(ISocialActionRepository));
                services.Remove(dbContextDescriptor);
                
                services.AddScoped<ISocialActionRepository>(_ =>
                {
                    var socialActionRepository = Substitute.For<ISocialActionRepository>();
                    socialActionRepository.GetAllSocialActions().Throws(new TaskCanceledException("Hello Exception"));
                    return socialActionRepository;
                });
            });
        }).CreateClient();
        
        // Act
        var response = await client.GetAsync("/api/v1.0/actions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var errorResponse = await response.Content.ReadFromJsonAsync<IEnumerable<Error>>();

        var error = errorResponse!.First();
        error.Should().NotBeNull();
        error.Code.Should().Be("InternalError");
        error.Message.Should().Be("Hello Exception");
    }
}