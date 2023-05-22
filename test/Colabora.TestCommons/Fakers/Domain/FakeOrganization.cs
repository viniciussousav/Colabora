using System.Linq;
using Bogus;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers.Domain;

public static class FakerOrganization
{
    private static readonly Faker Faker = new();

    public static Organization Create()
    {
        return new Organization(
            name: Faker.Random.Word(),
            email: Faker.Person.Email,
            state: Faker.Random.Enum(exclude: States.Undefined),
            interests: Faker.Random.EnumValues(exclude: Interests.Undefined).ToList(),
            createdBy: Faker.Random.Int(min: 1),
            verified: false);
    }
    
    public static Organization Create(RegisterOrganizationCommand command)
    {
        return new Organization(
            name: command.Name,
            email: command.Email,
            state: command.State,
            interests: command.Interests,
            createdBy: command.VolunteerCreatorId,
            verified: false);
    }
}