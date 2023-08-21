using System;
using Bogus;
using Colabora.Application.Features.Volunteer.RegisterOrganization.Models;
using Colabora.Domain.Shared.Enums;

namespace Colabora.TestCommons.Fakers.Commands;

public static class FakeRegisterOrganizationCommand
{
    public static RegisterOrganizationCommand Create(Guid? volunteerCreatorId = null)
    {
        var faker = new Faker();
        
        return new RegisterOrganizationCommand(
            Name: faker.Person.FirstName,
            Email: faker.Person.Email,
            State: faker.Random.Enum(exclude: States.Undefined),
            Interests: faker.Random.EnumValues(exclude: Interests.Undefined, count: faker.Random.Int(1, 5)),
            VolunteerCreatorId: volunteerCreatorId ?? Guid.NewGuid());
    }
}