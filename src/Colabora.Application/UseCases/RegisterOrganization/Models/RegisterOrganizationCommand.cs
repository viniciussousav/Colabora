using Colabora.Application.Commons;
using Colabora.Domain.Enums;
using MediatR;

namespace Colabora.Application.UseCases.RegisterOrganization.Models;

public record RegisterOrganizationCommand(
    string Name,
    string Email,
    States State,
    List<Interests> Interests,
    int CreatedBy
) : IRequest<Result<RegisterOrganizationResponse>>;
