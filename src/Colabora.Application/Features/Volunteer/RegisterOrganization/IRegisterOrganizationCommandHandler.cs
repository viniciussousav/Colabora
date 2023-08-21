using Colabora.Application.Commons;
using Colabora.Application.Features.Volunteer.RegisterOrganization.Models;
using MediatR;

namespace Colabora.Application.Features.Volunteer.RegisterOrganization;

public interface IRegisterOrganizationCommandHandler : IRequestHandler<RegisterOrganizationCommand, Result<RegisterOrganizationResponse>> { }