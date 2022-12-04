using System;
using Bogus;
using Colabora.Application.Organizations.RegisterOrganization.Models;
using Colabora.Domain.Entities;

namespace Colabora.TestCommons.Fakers;

public static class FakerOrganization
{
    private static readonly Faker Faker = new();

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