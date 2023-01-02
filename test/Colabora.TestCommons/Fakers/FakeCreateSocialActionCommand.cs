using System;
using System.Linq;
using Bogus;
using Colabora.Application.UseCases.CreateSocialAction.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.Enums;

namespace Colabora.TestCommons.Fakers;

public class FakeCreateSocialActionCommand
{
    private static readonly Faker Faker = new();
    
    public static CreateSocialActionCommand Create()
    {
        return new CreateSocialActionCommand(
            Title: Faker.Random.Words(),
            Fields: Faker.Random.EnumValues(exclude: Interests.Undefined).ToList(),
            OccurrenceDate: DateTime.Now,
            State: Faker.Random.Enum(exclude: States.Undefined),
            Description: Faker.Random.Words(),
            OrganizationId: Faker.Random.Int(min: 1),
            VolunteerCreatorId: Faker.Random.Int(min: 1),
            Organizers: Array.Empty<Volunteer>().ToList(),
            Participants: Array.Empty<Volunteer>().ToList());
    }
}