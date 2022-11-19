using Colabora.Application.Handlers.Volunteers.RegisterVolunteer.Models;
using Colabora.Domain.Entities;

namespace Colabora.Application.Handlers.Volunteers.RegisterVolunteer.Mappers;

public static class RegisterVolunteerCommandMapper
{
    public static Volunteer MapToVolunteer(this RegisterVolunteerCommand command)
        => new(
            id: default,
            firstName: command.FirstName,
            lastName: command.LastName,
            email: command.Email,
            birthdate: command.Birthdate,
            gender: command.Gender,
            interests: command.Interests,
            state: command.State,
            createAt: DateTime.Now);
}