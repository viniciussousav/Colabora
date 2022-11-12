using Colabora.Application.Shared;
using Colabora.Application.UseCases.Organizations.CreateOrganization.Models;

namespace Colabora.Application.UseCases.Organizations.CreateOrganization;

public interface ICreateOrganizationUseCase
{
    Task<Result> CreateOrganization(CreateOrganizationCommand command);
}