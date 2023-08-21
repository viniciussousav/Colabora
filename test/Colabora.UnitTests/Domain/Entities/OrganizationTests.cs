using System;
using System.Linq;
using Bogus;
using Colabora.Domain.Organization;
using Colabora.Domain.Shared.Enums;
using FluentAssertions;
using Xunit;

namespace Colabora.UnitTests.Domain.Entities;

public class OrganizationTests
{
    private static readonly Faker Faker = new ();
    
    [Fact(DisplayName = "Given an new Organization, when all parameters are passed, it should be succeed")]
    public void Given_A_New_Volunteer_Instance_When_All_Parameters_Are_Passed_It_Should_Be_Succeed()
    {
        // Arrange
        var (organizationId, name, email, state, interests, createdBy) = (
            Guid.NewGuid(),
            Faker.Company.CompanyName(),
            Faker.Person.Email,
            Faker.Random.Enum(exclude: States.Undefined),
            Faker.Random.EnumValues<Interests>().ToList(),
            Faker.Random.Int());

        // Act
        var organization = new Organization(organizationId, name, email, state, interests);
        
        // Assert
        organization.VolunteerCreatorId.Should().Be(organizationId);
        organization.Name.Should().Be(name);
        organization.Email.Should().Be(email);
        organization.State.Should().Be(state);
        organization.Interests.Should().BeEquivalentTo(interests);
    }
}