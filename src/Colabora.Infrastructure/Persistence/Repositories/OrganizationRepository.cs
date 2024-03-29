﻿using Colabora.Domain.Organization;
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
    
    public async Task<Organization> GetOrganization(string name, string email, Guid volunteerCreatorId)
    {
        return await _appDbContext.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => 
                o.Name == name &&
                o.Email == email &&
                o.VolunteerCreatorId == volunteerCreatorId) ?? Organization.None;
    }

    public async Task<Organization> GetOrganizationById(Guid organizationId, bool includeSocialActions = false)
    {
        if (includeSocialActions)
        {
            return await _appDbContext.Organizations
                .AsNoTracking()
                .Include(organization => organization.SocialActions)
                .FirstOrDefaultAsync(o => o.Id == organizationId) ?? Organization.None;
        }
        
        return await _appDbContext.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == organizationId) ?? Organization.None;
    }
}