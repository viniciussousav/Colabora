using Colabora.Application.Commons;
using Colabora.Application.Handlers.Organizations.RegisterOrganization.Models;
using MediatR;

namespace Colabora.Application.Handlers.Organizations.RegisterOrganization;

public interface IRegisterOrganizationCommandHandler : IRequestHandler<RegisterOrganizationCommand, Result<RegisterOrganizationResponse>> { }