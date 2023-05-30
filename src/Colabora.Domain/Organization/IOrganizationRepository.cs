namespace Colabora.Domain.Organization;

public interface IOrganizationRepository
{
    Task<Organization> CreateOrganization(Organization organization);
    Task<List<Organization>> GetAllOrganizations();
    Task<Organization> GetOrganization(string name, string email, int volunteerCreatorId);
    Task<Organization> GetOrganizationById(int organizationId, bool includeSocialActions = false);
    Task UpdateOrganization(Organization organization);
}