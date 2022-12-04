namespace Colabora.Application.Volunteers.GetVolunteers.Models;

public record GetVolunteersResponse(
    List<VolunteerResponse> Volunteers
);