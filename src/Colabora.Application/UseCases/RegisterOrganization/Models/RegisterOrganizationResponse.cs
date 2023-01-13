using Colabora.Domain.Enums;

namespace Colabora.Application.UseCases.RegisterOrganization.Models;

public record RegisterOrganizationResponse(
    int Id,
    string Name,
    States State,
    List<Interests> Interests,
    int CreatedBy,
    DateTime CreatedAt);