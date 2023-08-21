using Colabora.Domain.Shared.Interfaces;

namespace Colabora.Domain.Volunteer;

public interface IVolunteerRepository : IRepository<Volunteer>
{
    Task<Volunteer> CreateVolunteer(Volunteer volunteer);
    Task<Volunteer> GetVolunteerByEmail(string email);
    Task<List<Volunteer>> GetAllVolunteers();
    Task<Volunteer> GetVolunteerById(Guid volunteerId, bool includeParticipations = false);
}