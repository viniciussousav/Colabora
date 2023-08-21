using Colabora.Application.Features.Volunteer.GetVolunteerById.Models;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Volunteer;

namespace Colabora.Application.Mappers;

public static class VolunteerMapper
{
    public static GetVolunteersItemResponse MapToGetVolunteerItemResponse(this Volunteer volunteer)
    {
        return new GetVolunteersItemResponse(
            VolunteerId: volunteer.Id,
            FirstName: volunteer.FirstName,
            LastName: volunteer.LastName,
            Email: volunteer.Email,
            State: volunteer.State,
            Gender: volunteer.Gender,
            Interests: volunteer.Interests,
            Birthdate: volunteer.Birthdate,
            CreatedAt: volunteer.CreateAt.ToUniversalTime());
    }
    
    public static RegisterVolunteerResponse MapToRegisterVolunteerResponse(this Volunteer volunteer)
    {
        return new RegisterVolunteerResponse(
            VolunteerId: volunteer.Id,
            FirstName: volunteer.FirstName,
            LastName: volunteer.LastName,
            Email: volunteer.Email,
            State: volunteer.State,
            Gender: volunteer.Gender,
            Interests: volunteer.Interests,
            Birthdate: volunteer.Birthdate.ToUniversalTime(),
            CreatedAt: volunteer.CreateAt.ToUniversalTime());
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
            birthdate: command.Birthdate.ToUniversalTime());
    }
    
    public static GetVolunteerByIdResponse MapToGetVolunteerByIdResponse(this Volunteer volunteer)
    {
        return new GetVolunteerByIdResponse(
            VolunteerId: volunteer.Id,
            FirstName: volunteer.FirstName,
            LastName: volunteer.LastName,
            Email: volunteer.Email,
            State: volunteer.State,
            Gender: volunteer.Gender,
            Interests: volunteer.Interests,
            Birthdate: volunteer.Birthdate.ToUniversalTime(),
            CreatedAt: volunteer.CreateAt.ToUniversalTime(),
            Participations: volunteer.Participations.Select(
                participation => new VolunteerParticipationDetails(
                    SocialActionId: participation.SocialActionId,
                    SocialActionTitle: participation.SocialAction.Title,
                    JoinedAt: participation.JoinedAt.ToUniversalTime(),
                    OccurrenceDate: participation.SocialAction.OccurrenceDate)).ToList());
    }
}