using Colabora.Domain.Entities;

namespace Colabora.Domain.Repositories;

public interface IVolunteerRepository
{
    Task<Volunteer> CreateVolunteer(Volunteer volunteer);
    Task<Volunteer> GetVolunteerByEmail(string email);
    Task<List<Volunteer>> GetAllVolunteers();
    Task<Volunteer> GetVolunteerById(int volunteerId, bool includeParticipations = false);
}