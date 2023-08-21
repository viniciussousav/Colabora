using Colabora.Domain.Shared.Interfaces;

namespace Colabora.Domain.Organization;

public interface IOrganizationRepository : IRepository<Organization>
{
    Task<Organization> CreateOrganization(Organization organization);
    Task<Organization> GetOrganization(string name, string email, Guid volunteerCreatorId);
    Task<Organization> GetOrganizationById(Guid organizationId, bool includeSocialActions = false);
}