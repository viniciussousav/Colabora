using System;
using Bogus;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Shared.Enums;
using Colabora.Domain.Volunteer;

namespace Colabora.TestCommons.Fakers.Commands;

public static class FakeRegisterVolunteerCommand
{
    public static RegisterVolunteerCommand CreateValid(string? email = null)
    {
        var faker = new Faker();
        return new RegisterVolunteerCommand
        {
            FirstName = faker.Person.FirstName,
            LastName = faker.Person.LastName,
            Email = email ?? faker.Person.Email,
            State = faker.Random.Enum(exclude: States.Undefined),
            Gender = faker.Random.Enum(exclude: Gender.Undefined),
            Interests = faker.Random.EnumValues(exclude: Interests.Undefined, count: faker.Random.Int(1, 5)),
            Birthdate = DateTimeOffset.Now.AddYears(-20)
        };
    }
}