using Colabora.Domain.Entities;

namespace Colabora.Domain.Repositories;

public interface IOrganizationRepository
{
    Task<Organization> CreateOrganization(Organization organization);
    Task<List<Organization>> GetAllOrganizations();
    Task<Organization> GetOrganizationByNameAndCreator(string name, int volunteerCreatorId);
    Task<Organization> GetOrganizationById(int organizationId);
}