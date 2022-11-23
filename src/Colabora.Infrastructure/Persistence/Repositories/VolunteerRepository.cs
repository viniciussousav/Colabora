using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Colabora.Infrastructure.Persistence.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public VolunteerRepository(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    public async Task<Volunteer> CreateVolunteerAsync(Volunteer volunteer)
    {
        var ctx = await _contextFactory.CreateDbContextAsync();
        var entry = await ctx.Volunteers.AddAsync(volunteer);
        return entry.Entity;
    }

    public async Task<Volunteer> GetVolunteerByEmailAsync(string email)
    {
        var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Volunteers
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Email == email) ?? Volunteer.Empty;

    }

    public async Task<List<Volunteer>> GetAllVolunteersAsync()
    {
        var ctx = await _contextFactory.CreateDbContextAsync();
        return await ctx.Volunteers
            .AsNoTracking()
            .ToListAsync();
    }
}