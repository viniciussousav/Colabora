using System.Linq;
using Bogus;
using Colabora.Application.UseCases.RegisterOrganization.Models;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public static class FakeRegisterOrganizationCommand
{
    private static readonly Faker Faker = new();

    public static RegisterOrganizationCommand Create()
    {
        return new RegisterOrganizationCommand(
            Name: Faker.Company.CompanyName(),
            Email: Faker.Person.Email,
            State: Faker.Random.Enum(exclude: States.Undefined),
            Interests: Faker.Random.EnumValues<Interests>().ToList(),
            CreatedBy: Faker.Random.Int());
    }
}