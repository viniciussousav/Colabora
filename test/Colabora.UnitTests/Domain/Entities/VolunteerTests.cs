﻿using System.Linq;
using Bogus;
using Colabora.Domain.Shared.Enums;
using Colabora.Domain.Volunteer;
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
        var (firstName, lastName, email, dateOfBirth,gender, interests, state) = (
            Faker.Person.FirstName,
            Faker.Person.LastName,
            Faker.Person.Email,
            Faker.Person.DateOfBirth,
            Faker.Random.Enum(exclude: Gender.Undefined),
            Faker.Random.EnumValues<Interests>().ToList(),
            Faker.Random.Enum(exclude: States.Undefined));
                    
        // Act
        var volunteer = new Volunteer(firstName, lastName, email, dateOfBirth, gender, interests, state);
        
        // Assert
        volunteer.FirstName.Should().Be(firstName);
        volunteer.LastName.Should().Be(lastName);
        volunteer.Gender.Should().Be(gender);
        volunteer.Interests.Should().BeEquivalentTo(interests);
        volunteer.Email.Should().Be(email);
        volunteer.State.Should().Be(state);
        volunteer.Birthdate.Should().Be(dateOfBirth);
    }
}