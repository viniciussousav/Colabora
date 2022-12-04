using Colabora.Application.Commons;
using Colabora.Application.Organizations.RegisterOrganization.Models;
using MediatR;

namespace Colabora.Application.Organizations.RegisterOrganization;

public interface IRegisterOrganizationCommandHandler : IRequestHandler<RegisterOrganizationCommand, Result<RegisterOrganizationResponse>> { }