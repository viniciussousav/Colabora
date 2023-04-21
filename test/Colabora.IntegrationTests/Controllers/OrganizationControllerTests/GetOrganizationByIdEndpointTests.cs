using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Application.Shared;
using Colabora.Domain.Repositories;
using Colabora.TestCommons.Fakers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

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
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var registerVolunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await registerVolunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
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
        getOrganizationByIdResponse.CreatedBy.Should().Be(organization.CreatedBy).And.Be(volunteer.VolunteerId);
        getOrganizationByIdResponse.CreatedAt.AddHours(-3).Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        getOrganizationByIdResponse.Interests.Should().BeEquivalentTo(organization.Interests);
        getOrganizationByIdResponse.SocialActions.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Given_A_Get_Organization_By_Id_Request_When_Organization_Exists_With_SocialActions_Then_It_Should_Return_The_Existing_Organization()
    {
        
        
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var registerVolunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await registerVolunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();
        
        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteer.VolunteerId);
        var registerOrganizationResponse = await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        var organization = await registerOrganizationResponse.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();
        
        var createSocialActionCommand = FakeCreateSocialActionCommand.Create(organization.OrganizationId, volunteer.VolunteerId);
        var createSocialActionResponse = await client.PostAsJsonAsync("api/v1.0/actions/", createSocialActionCommand);
        var socialAction = await createSocialActionResponse.Content.ReadFromJsonAsync<CreateSocialActionResponse>();
        
        // Act 
        var response = await client.GetAsync($"/api/v1.0/organizations/{organization.OrganizationId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getOrganizationByIdResponse = await response.Content.ReadFromJsonAsync<GetOrganizationByIdResponse>();

        getOrganizationByIdResponse.Should().NotBeNull();
        getOrganizationByIdResponse.OrganizationId.Should().Be(organization.OrganizationId);
        getOrganizationByIdResponse.Name.Should().BeEquivalentTo(organization.Name);
        getOrganizationByIdResponse.State.Should().Be(organization.State);
        getOrganizationByIdResponse.CreatedBy.Should().Be(organization.CreatedBy).And.Be(volunteer.VolunteerId);
        getOrganizationByIdResponse.CreatedAt.AddHours(-3).Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        getOrganizationByIdResponse.Interests.Should().BeEquivalentTo(organization.Interests);
        
        getOrganizationByIdResponse.SocialActions.Should().HaveCount(1);
        var firstSocialAction = getOrganizationByIdResponse.SocialActions.First();
        firstSocialAction.SocialActionId.Should().Be(socialAction.SocialActionId);
        firstSocialAction.SocialActionTitle.Should().Be(socialAction.Title);
        firstSocialAction.CreatedAt.Should().Be(socialAction.CreatedAt);
        firstSocialAction.OccurrenceDate.Should().Be(socialAction.OccurrenceDate);
    }

    [Fact]
    public async Task
        Given_A_Get_Organization_By_Id_Request_When_An_Exception_Occurs_Then_It_Should_Return_An_Error_With_500_Status_Code()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var registerVolunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await registerVolunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();

        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteer.VolunteerId);
        var registerOrganizationResponse =
            await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
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
                    mockRepository.GetOrganizationById(Arg.Any<int>(), Arg.Any<bool>())
                        .Throws(new TaskCanceledException("Hello Exception"));
                    return mockRepository;
                });
            });
        }).CreateClient();

        // Act 
        var response = await client.GetAsync($"/api/v1.0/organizations/{organization.OrganizationId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var error = await response.Content.ReadFromJsonAsync<IEnumerable<Error>>();
        error.Should().ContainEquivalentOf(ErrorMessages.CreateInternalError("Hello Exception"));
    }
}