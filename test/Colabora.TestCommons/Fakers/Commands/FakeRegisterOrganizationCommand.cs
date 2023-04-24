using Bogus;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers.Commands;

public static class FakeRegisterOrganizationCommand
{
    public static RegisterOrganizationCommand Create(int? volunteerCreatorId = null)
    {
        var faker = new Faker();
        
        return new RegisterOrganizationCommand(
            Name: faker.Person.FirstName,
            Email: faker.Person.Email,
            State: faker.Random.Enum(exclude: States.Undefined),
            Interests: faker.Random.EnumValues(exclude: Interests.Undefined, count: faker.Random.Int(1, 5)),
            VolunteerCreatorId: volunteerCreatorId ?? faker.Random.Int(min: 1));
    }
}