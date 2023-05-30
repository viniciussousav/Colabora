namespace Colabora.Domain.Volunteer;

public interface IVolunteerRepository
{
    Task<Volunteer> CreateVolunteer(Volunteer volunteer);
    Task<Volunteer> GetVolunteerByEmail(string email);
    Task<List<Volunteer>> GetAllVolunteers();
    Task<Volunteer> GetVolunteerById(int volunteerId, bool includeParticipations = false);
}