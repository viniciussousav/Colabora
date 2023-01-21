using System.Linq;
using Bogus;
using Colabora.Application.UseCases.CreateSocialAction.Models;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public static class FakeCreateSocialActionCommand
{
    private static readonly Faker Faker = new();
    
    public static CreateSocialActionCommand Create()
    {
        return new CreateSocialActionCommand(
            Title: Faker.Random.Word(),
            Description: Faker.Random.Word(),
            OrganizationId: Faker.Random.Int(min: 1),
            VolunteerCreatorId: Faker.Random.Int(min: 1),
            State: Faker.Random.Enum(exclude: States.Undefined),
            Interests: Faker.Random.EnumValues(exclude: Interests.Undefined).ToList(),
            OccurrenceDate: Faker.Date.Future());
    }
}