using Colabora.Application.Handlers.Volunteers.RegisterVolunteer.Models;
using Colabora.Domain.Entities;

namespace Colabora.Application.Handlers.Volunteers;

public static class VolunteerMapper
{
    public static RegisterVolunteerResponse MapToRegisterVolunteerResponse(this Volunteer volunteer)
        => new(
            volunteer.Id,
            volunteer.FirstName,
            volunteer.LastName,
            volunteer.Email,
            volunteer.State,
            volunteer.Gender,
            volunteer.Interests,
            volunteer.Birthdate,
            volunteer.CreateAt);
    
    public static VolunteerResponse MapToGetVolunteerResponse(this Volunteer volunteer)
        => new(
            volunteer.Id,
            volunteer.FirstName,
            volunteer.LastName,
            volunteer.Email,
            volunteer.State,
            volunteer.Gender,
            volunteer.Interests,
            volunteer.Birthdate,
            volunteer.CreateAt);
}