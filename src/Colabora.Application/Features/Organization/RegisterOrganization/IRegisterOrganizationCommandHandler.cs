using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using MediatR;

namespace Colabora.Application.Features.Organization.RegisterOrganization;

public interface IRegisterOrganizationCommandHandler : IRequestHandler<RegisterOrganizationCommand, Result<RegisterOrganizationResponse>> { }