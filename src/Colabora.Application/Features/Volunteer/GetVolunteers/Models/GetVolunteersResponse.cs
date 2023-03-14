namespace Colabora.Application.Features.Volunteer.GetVolunteers.Models;

public record GetVolunteersResponse(
    List<GetVolunteersItemResponse> Volunteers
);