using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Colabora.Infrastructure.Persistence.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly AppDbContext _appDbContext;

    public VolunteerRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task<Volunteer> CreateVolunteer(Volunteer volunteer)
    {
        var entry = await _appDbContext.Volunteers.AddAsync(volunteer);
        await _appDbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<Volunteer> GetVolunteerByEmail(string email)
    {
        return await _appDbContext.Volunteers
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Email == email) ?? Volunteer.None;
    }
    
    public async Task<Volunteer> GetVolunteerById(int volunteerId, bool includeParticipations = false)
    {
        if (!includeParticipations)
        {
            return await _appDbContext.Volunteers
                .AsNoTracking()
                .Include(volunteer => volunteer.Participations)
                .FirstOrDefaultAsync(v => v.VolunteerId == volunteerId) ?? Volunteer.None;
        }
        
        return await _appDbContext.Volunteers
            .AsNoTracking()
            .Include(volunteer => volunteer.Participations)
            .ThenInclude(participation => participation.SocialAction)
            .FirstOrDefaultAsync(v => v.VolunteerId == volunteerId) ?? Volunteer.None;
    }
    
    public async Task<List<Volunteer>> GetAllVolunteers()
    {
        return await _appDbContext.Volunteers
            .AsNoTracking()
            .ToListAsync();
    }
}