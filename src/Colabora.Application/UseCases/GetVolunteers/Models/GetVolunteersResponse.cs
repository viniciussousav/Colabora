namespace Colabora.Application.UseCases.GetVolunteers.Models;

public record GetVolunteersResponse(
    List<GetVolunteersItemResponse> Volunteers
);