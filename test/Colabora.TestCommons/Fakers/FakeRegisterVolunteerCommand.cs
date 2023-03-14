using System.Linq;
using Bogus;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public static class FakeRegisterVolunteerCommand
{
    private static readonly Faker Faker = new();

    public static RegisterVolunteerCommand Create(string? email = null)
    {
        return new RegisterVolunteerCommand(
            FirstName: Faker.Random.Word(),
            LastName: Faker.Random.Word(),
            Email: email ?? Faker.Random.Word() + "@email.com",
            State: Faker.Random.Enum(exclude: States.Undefined),
            Gender: Faker.Random.Enum(exclude: Gender.Undefined),
            Interests: Faker.Random.EnumValues(exclude: Interests.Undefined).ToList(),
            Birthdate: Faker.Date.Past());
    }
}