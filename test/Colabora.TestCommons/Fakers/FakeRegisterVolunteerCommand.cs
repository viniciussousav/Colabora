using Bogus;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public static class FakeRegisterVolunteerCommand
{
    

    public static RegisterVolunteerCommand Create(string? email = null)
    {
        var  faker = new Faker();
        return new RegisterVolunteerCommand(
            FirstName: faker.Person.FirstName,
            LastName: faker.Person.LastName,
            Email: email ?? faker.Person.Email,
            State: faker.Random.Enum(exclude: States.Undefined),
            Gender: faker.Random.Enum(exclude: Gender.Undefined),
            Interests: faker.Random.EnumValues(exclude: Interests.Undefined),
            Birthdate: faker.Date.Past(yearsToGoBack: 20));
    }
}