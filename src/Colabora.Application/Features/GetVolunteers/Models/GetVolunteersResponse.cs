namespace Colabora.Application.Features.GetVolunteers.Models;

public record GetVolunteersResponse(
    List<GetVolunteersItemResponse> Volunteers
);