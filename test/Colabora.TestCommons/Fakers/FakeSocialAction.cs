using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Colabora.Domain.Entities;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public static class FakeSocialAction
{
    private static readonly Faker Faker = new();

    public static SocialAction Create(int? organizationId = null, int? volunteerCreatorId = null)
    {
        return new SocialAction(
            title: Faker.Random.Word(),
            description: Faker.Random.String(minLength: 10, maxLength: 255),
            organizationId: organizationId ?? Faker.Random.Int(min: 1),
            volunteerCreatorId: volunteerCreatorId ?? Faker.Random.Int(min: 1),
            state: Faker.Random.Enum(exclude: States.Undefined),
            interests: Faker.Random.EnumValues(exclude: Interests.Undefined).ToList(),
            participations: new List<Participation>(),
            occurrenceDate: Faker.Date.Future(),
            createdAt: DateTimeOffset.Now);
    }
}