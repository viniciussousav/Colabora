namespace Colabora.Application.Handlers.Volunteers.GetVolunteers.Models;

public record GetVolunteersResponse(
    List<VolunteerResponse> Volunteers
);