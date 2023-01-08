using System;
using System.Linq;
using Bogus;
using Colabora.Domain.Entities;
using Colabora.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Colabora.UnitTests.Domain.Entities;


public class VolunteerTests
{
    private static readonly Faker Faker = new ();
    
    [Fact(DisplayName = "Given an new Volunteer instance, when all parameters are passed, it should be succeed")]
    public void Given_A_New_Volunteer_Instance_When_All_Parameters_Are_Passed_It_Should_Be_Succeed()
    {
        // Arrange
        var (id, firstName, lastName, email, dateOfBirth,gender, interests, state, createdAt) = (
            Faker.Random.Int(),
            Faker.Person.FirstName,
            Faker.Person.LastName,
            Faker.Person.Email,
            Faker.Person.DateOfBirth,
            Faker.Random.Enum(exclude: Gender.Undefined),
            Faker.Random.EnumValues<Interests>().ToList(),
            Faker.Random.Enum(exclude: States.Undefined),
            DateTime.Now);
                    
        // Act
        var volunteer = new Volunteer(id, firstName, lastName, email, dateOfBirth, gender, interests, state, createdAt);
        
        // Assert
        volunteer.VolunteerId.Should().Be(id);
        volunteer.FirstName.Should().Be(firstName);
        volunteer.LastName.Should().Be(lastName);
        volunteer.Gender.Should().Be(gender);
        volunteer.Interests.Should().BeEquivalentTo(interests);
        volunteer.Email.Should().Be(email);
        volunteer.State.Should().Be(state);
        volunteer.Birthdate.Should().Be(dateOfBirth);
        volunteer.CreateAt.Should().Be(createdAt);
    }
}