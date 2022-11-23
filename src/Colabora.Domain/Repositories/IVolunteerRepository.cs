using Colabora.Domain.Entities;

namespace Colabora.Domain.Repositories;

public interface IVolunteerRepository
{
    Task<Volunteer> CreateVolunteerAsync(Volunteer volunteer);
    Task<Volunteer> GetVolunteerByEmailAsync(string email);
    Task<List<Volunteer>> GetAllVolunteersAsync();
}