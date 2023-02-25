using Colabora.Application.Features.GetVolunteers.Models;
using Colabora.Application.Features.RegisterVolunteer.Models;
using Colabora.Domain.Entities;

namespace Colabora.Application.Mappers;

public static class VolunteerMapper
{
    public static GetVolunteersItemResponse MapToGetVolunteerItemResponse(this Volunteer volunteer)
    {
        return new GetVolunteersItemResponse(
            VolunteerId: volunteer.VolunteerId,
            FirstName: volunteer.FirstName,
            LastName: volunteer.LastName,
            Email: volunteer.Email,
            State: volunteer.State,
            Gender: volunteer.Gender,
            Interests: volunteer.Interests,
            Birthdate: volunteer.Birthdate,
            CreatedAt: volunteer.CreateAt);
    }
    
    public static RegisterVolunteerResponse MapToRegisterVolunteerResponse(this Volunteer volunteer)
    {
        return new RegisterVolunteerResponse(
            VolunteerId: volunteer.VolunteerId,
            FirstName: volunteer.FirstName,
            LastName: volunteer.LastName,
            Email: volunteer.Email,
            State: volunteer.State,
            Gender: volunteer.Gender,
            Interests: volunteer.Interests,
            Birthdate: volunteer.Birthdate,
            CreatedAt: volunteer.CreateAt);
    }

    public static Volunteer MapToVolunteer(this RegisterVolunteerCommand command)
    {
        return new Volunteer(
            firstName: command.FirstName,
            lastName: command.LastName,
            email: command.Email,
            state: command.State,
            gender: command.Gender,
            interests: command.Interests,
            birthdate: command.Birthdate);
    }
}