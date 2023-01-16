using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Colabora.Infrastructure.Persistence.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly AppDbContext _appDbContext;

    public OrganizationRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Organization> CreateOrganization(Organization organization)
    {
        var entry = await _appDbContext.Organizations.AddAsync(organization);
        await _appDbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<List<Organization>> GetAllOrganizations()
    {
        return await _appDbContext.Organizations.AsNoTracking().ToListAsync();
    }
    
    public async Task<Organization> GetOrganization(string name, string email, int volunteerCreatorId)
    {
        return await _appDbContext.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Name == name && o.CreatedBy == volunteerCreatorId) ?? Organization.None;
    }

    public async Task<Organization> GetOrganizationById(int organizationId)
    {
        return await _appDbContext.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == organizationId) ?? Organization.None;
    }
}