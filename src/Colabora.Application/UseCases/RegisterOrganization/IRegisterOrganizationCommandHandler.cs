using Colabora.Application.Commons;
using Colabora.Application.UseCases.RegisterOrganization.Models;
using MediatR;

namespace Colabora.Application.UseCases.RegisterOrganization;

public interface IRegisterOrganizationCommandHandler : IRequestHandler<RegisterOrganizationCommand, Result<RegisterOrganizationResponse>> { }