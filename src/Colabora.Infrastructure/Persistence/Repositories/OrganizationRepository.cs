using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Colabora.Infrastructure.Persistence.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public OrganizationRepository(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Organization> CreateOrganization(Organization organization)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        var entry = await ctx.Organizations.AddAsync(organization);
        return entry.Entity;
    }

    public async Task<List<Organization>> GetAllOrganizations()
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return ctx.Organizations.AsNoTracking().ToList();
    }

    public async Task<Organization> GetOrganizationByNameAndCreator(string name, int volunteerCreatorId)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Name == name && o.CreatedBy == volunteerCreatorId) ?? Organization.None;
    }

    public async Task<Organization> GetOrganizationById(int organizationId)
    {
        await using var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == organizationId) ?? Organization.None;
    }
}