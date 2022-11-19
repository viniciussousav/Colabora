using System;
using Bogus;
using Colabora.Application.Handlers.Organizations.RegisterOrganization.Models;

namespace Colabora.TestCommons.Fakers;

public static class FakerOrganization
{
    private static readonly Faker Faker = new();

    public static Domain.Entities.Organization Create(RegisterOrganizationCommand command)
    {
        return new Domain.Entities.Organization(
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