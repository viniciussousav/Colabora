using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Features.SocialAction.JoinSocialAction.Models;
using Colabora.Application.Features.Volunteer.GetVolunteerById.Models;
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

namespace Colabora.IntegrationTests.Controllers.VolunteerControllerTests;

public partial class VolunteerControllerTests
{
    [Fact]
    public async Task Given_A_Get_Volunteer_Request_When_Volunteer_Not_Exists_Then_It_Should_Return_An_Error_With_404_StatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"api/v1.0/volunteers/{123413}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var body = await response.Content.ReadFromJsonAsync<Error>();
        body.Should().NotBeNull();
        body.Should().BeEquivalentTo(ErrorMessages.CreateVolunteerNotFound());
    }
    
    [Fact]
    public async Task Given_A_Get_Volunteer_Request_When_Volunteer_Exists_Then_It_Should_Return_The_Existing_Volunteer()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();

        // Act
        var response = await client.GetAsync($"api/v1.0/volunteers/{volunteer.VolunteerId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetVolunteerByIdResponse>();
        body.VolunteerId.Should().BePositive();
        body.Birthdate.Should().Be(registerVolunteerCommand.Birthdate);
        body.Email.Should().Be(registerVolunteerCommand.Email);
        body.Gender.Should().Be(registerVolunteerCommand.Gender);
        body.Interests.Should().BeEquivalentTo(registerVolunteerCommand.Interests);
        body.State.Should().Be(registerVolunteerCommand.State);
        body.FirstName.Should().Be(registerVolunteerCommand.FirstName);
        body.LastName.Should().Be(registerVolunteerCommand.LastName);
        body.CreatedAt.AddHours(-3).Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        body.Participations.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Given_A_Get_Volunteer_Request_When_Volunteer_Exists_And_Has_Participations_Then_It_Should_Return_The_Existing_Volunteer_With_Participations()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();

        var registerOrganizationCommand = FakeRegisterOrganizationCommand.Create(volunteerCreatorId: volunteer.VolunteerId);
        var organizationResponse = await client.PostAsJsonAsync("/api/v1.0/organizations", registerOrganizationCommand);
        var organization = await organizationResponse.Content.ReadFromJsonAsync<RegisterOrganizationResponse>();
        
        var createSocialActionCommand = FakeCreateSocialActionCommand.Create(organization.OrganizationId, volunteer.VolunteerId);
        var createSocialActionResponse = await client.PostAsJsonAsync("api/v1.0/actions", createSocialActionCommand);
        var socialAction = await createSocialActionResponse.Content.ReadFromJsonAsync<CreateSocialActionResponse>();
        
        var joinSocialActionCommand = new JoinSocialActionCommand(socialAction.SocialActionId, volunteer.VolunteerId);
        await client.PostAsJsonAsync($"api/v1.0/actions/{socialAction.SocialActionId}/join", joinSocialActionCommand);

        // Act
        var response = await client.GetAsync($"api/v1.0/volunteers/{volunteer.VolunteerId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetVolunteerByIdResponse>();
        body.VolunteerId.Should().BePositive();
        body.Birthdate.Should().Be(registerVolunteerCommand.Birthdate);
        body.Email.Should().Be(registerVolunteerCommand.Email);
        body.Gender.Should().Be(registerVolunteerCommand.Gender);
        body.Interests.Should().BeEquivalentTo(registerVolunteerCommand.Interests);
        body.State.Should().Be(registerVolunteerCommand.State);
        body.FirstName.Should().Be(registerVolunteerCommand.FirstName);
        body.LastName.Should().Be(registerVolunteerCommand.LastName);
        body.CreatedAt.AddHours(-3).Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        
        body.Participations.Should().HaveCount(1);
        body.Participations.First().SocialActionId.Should().Be(socialAction.SocialActionId);
        body.Participations.First().SocialActionTitle.Should().Be(socialAction.Title);
        body.Participations.First().OccurrenceDate.Should().Be(socialAction.OccurrenceDate);
        body.Participations.First().JoinedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public async Task Given_A_Get_Volunteer_Request_When_An_Exception_Occurs_Then_It_Should_Return_An_Error_With_500_StatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        var registerVolunteerCommand = FakeRegisterVolunteerCommand.Create();
        var volunteerResponse = await client.PostAsJsonAsync("/api/v1.0/volunteers", registerVolunteerCommand);
        var volunteer = await volunteerResponse.Content.ReadFromJsonAsync<RegisterVolunteerResponse>();

        client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescription = services.First(service => service.ServiceType == typeof(IVolunteerRepository));
                services.Remove(dbContextDescription);

                services.AddScoped<IVolunteerRepository>(_ =>
                {
                    var mockRepository = Substitute.For<IVolunteerRepository>();
                    mockRepository.GetVolunteerById(Arg.Any<int>(), Arg.Any<bool>())
                        .Throws(new TaskCanceledException("Hello Exception"));
                    return mockRepository;
                });
            });
        }).CreateClient();
        
        // Act
        var response = await client.GetAsync($"api/v1.0/volunteers/{volunteer.VolunteerId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var body = await response.Content.ReadFromJsonAsync<Error>();
        body.Should().BeEquivalentTo(ErrorMessages.CreateInternalError("Hello Exception"));
    }
}