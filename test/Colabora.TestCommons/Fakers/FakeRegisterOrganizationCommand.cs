﻿using System.Linq;
using Bogus;
using Colabora.Application.Features.RegisterOrganization.Models;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public static class FakeRegisterOrganizationCommand
{
    private static readonly Faker Faker = new();

    public static RegisterOrganizationCommand Create(int? volunteerCreatorId = null)
    {
        return new RegisterOrganizationCommand(
            Name: Faker.Company.CompanyName(),
            Email: Faker.Person.Email,
            State: Faker.Random.Enum(exclude: States.Undefined),
            Interests: Faker.Random.EnumValues<Interests>().ToList(),
            VolunteerCreatorId: volunteerCreatorId ?? Faker.Random.Int());
    }
}