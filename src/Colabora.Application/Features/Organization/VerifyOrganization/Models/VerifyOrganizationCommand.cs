using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.Organization.VerifyOrganization.Models;

public record VerifyOrganizationCommand(
    int OrganizationId
) : IRequest<Result<VerifyOrganizationResponse>>;