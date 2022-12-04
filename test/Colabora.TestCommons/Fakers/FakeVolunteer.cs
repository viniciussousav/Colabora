using System;
using System.Linq;
using Bogus;
using Colabora.Application.Volunteers.RegisterVolunteer.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public static class FakeVolunteer
{
    private static readonly Faker Faker = new Faker(); 
        
    public static Volunteer Create()
    {
        return new Volunteer(
            id: Faker.Random.Int(),
            firstName: Faker.Person.FirstName,
            lastName: Faker.Person.LastName,
            email: Faker.Person.Email,
            birthdate: Faker.Person.DateOfBirth,
            gender: Faker.Random.Enum(exclude: Gender.Undefined),
            interests: Faker.Random.EnumValues(exclude: Interests.Undefined).ToList(),
            state: Faker.Random.Enum(exclude: States.Undefined),
            createAt: DateTime.Now);
    }
    public static Volunteer Create(RegisterVolunteerCommand command)
    {
        return new Volunteer(
            id: Faker.Random.Int(),
            firstName: command.FirstName,
            lastName: command.LastName,
            email: command.Email,
            birthdate: command.Birthdate,
            gender: command.Gender,
            interests: command.Interests,
            state: command.State,
            createAt: DateTime.Now);
    }
}