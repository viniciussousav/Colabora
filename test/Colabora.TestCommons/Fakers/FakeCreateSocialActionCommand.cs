using System.Linq;
using Bogus;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public static class FakeCreateSocialActionCommand
{
    private static readonly Faker Faker = new();
    
    public static CreateSocialActionCommand Create(int? organizationId = null, int? volunteerCreatorId = null)
    {
        return new CreateSocialActionCommand(
            Title: Faker.Random.Word(),
            Description: Faker.Random.Word(),
            OrganizationId: organizationId ?? Faker.Random.Int(min: 1),
            VolunteerCreatorId: volunteerCreatorId ?? Faker.Random.Int(min: 1),
            State: Faker.Random.Enum(exclude: States.Undefined),
            Interests: Faker.Random.EnumValues(exclude: Interests.Undefined, count: Faker.Random.Int(min: 1, max: 4)).ToList(),
            OccurrenceDate: Faker.Date.Future());
    }
}