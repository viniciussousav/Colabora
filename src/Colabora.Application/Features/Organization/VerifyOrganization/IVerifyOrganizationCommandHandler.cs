using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.VerifyOrganization.Models;
using MediatR;

namespace Colabora.Application.Features.Organization.VerifyOrganization;

public interface IVerifyOrganizationCommandHandler : IRequestHandler<VerifyOrganizationCommand, Result<VerifyOrganizationResponse>>
{
    
}