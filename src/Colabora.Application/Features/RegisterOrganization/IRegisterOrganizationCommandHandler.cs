using Colabora.Application.Commons;
using Colabora.Application.Features.RegisterOrganization.Models;
using MediatR;

namespace Colabora.Application.Features.RegisterOrganization;

public interface IRegisterOrganizationCommandHandler : IRequestHandler<RegisterOrganizationCommand, Result<RegisterOrganizationResponse>> { }