using System;
using System.Linq;
using Bogus;
using Colabora.Application.UseCases.RegisterOrganization.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public static class FakerOrganization
{
    private static readonly Faker Faker = new();

    public static Organization Create()
    {
        return new Organization(
            id: Faker.Random.Int(),
            name: Faker.Random.Word(),
            email: Faker.Person.Email,
            state: Faker.Random.Enum(exclude: States.Undefined),
            interests: Faker.Random.EnumValues(exclude: Interests.Undefined).ToList(),
            memberships: Array.Empty<Membership>().ToList(),
            createdBy: Faker.Random.Int(min: 1),
            createdAt: DateTime.Now);
    }
    
    public static Organization Create(RegisterOrganizationCommand command)
    {
        return new Organization(
            id: Faker.Random.Int(),
            name: command.Name,
            email: command.Email,
            state: command.State,
            interests: command.Interests,
            memberships: command.Memberships,
            createdBy: command.CreatedBy,
            createdAt: DateTime.Now);
    }
}