using Colabora.Domain.Entities;

namespace Colabora.Domain.Repositories;

public interface IOrganizationRepository
{
    Task<Organization> CreateOrganizationAsync(Organization organization);
    Task<List<Organization>> GetAllOrganizationsAsync();

    Task<Organization> GetOrganizationByNameAndCreator(string name, int volunteerCreatorId);
}