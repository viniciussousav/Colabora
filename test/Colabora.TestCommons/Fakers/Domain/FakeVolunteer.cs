using System.Linq;
using Bogus;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Shared.Enums;
using Colabora.Domain.Volunteer;

namespace Colabora.TestCommons.Fakers.Domain;

public static class FakeVolunteer
{
    private static readonly Faker Faker = new(); 
        
    public static Volunteer Create()
    {
        return new Volunteer(
            firstName: Faker.Person.FirstName,
            lastName: Faker.Person.LastName,
            email: Faker.Person.Email,
            birthdate: Faker.Person.DateOfBirth,
            gender: Faker.Random.Enum(exclude: Gender.Undefined),
            interests: Faker.Random.EnumValues(exclude: Interests.Undefined).ToList(),
            state: Faker.Random.Enum(exclude: States.Undefined));
    }
    public static Volunteer Create(RegisterVolunteerCommand command)
    {
        return new Volunteer(
            firstName: command.FirstName,
            lastName: command.LastName,
            email: command.Email,
            birthdate: command.Birthdate,
            gender: command.Gender,
            interests: command.Interests,
            state: command.State);
    }
}