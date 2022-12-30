using System.Linq;
using Bogus;
using Colabora.Application.UseCases.RegisterVolunteer.Models;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public static class FakeRegisterVolunteerCommand
{
    private static readonly Faker Faker = new();

    public static RegisterVolunteerCommand Create()
    {
        return new RegisterVolunteerCommand(
            FirstName: Faker.Person.FirstName,
            LastName: Faker.Person.LastName,
            Email: Faker.Person.Email,
            State: Faker.Random.Enum(exclude: States.Undefined),
            Gender: Faker.Random.Enum(exclude: Gender.Undefined),
            Interests: Faker.Random.EnumValues(exclude: Interests.Undefined).ToList(),
            Birthdate: Faker.Person.DateOfBirth);
    }
}