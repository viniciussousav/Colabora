using Colabora.Domain.Enums;

namespace Colabora.Domain.Entities;

public class Volunteer
{
    public static readonly Volunteer None = new();

    private Volunteer() { }
    
    public Volunteer(
        int volunteerId,
        string firstName, 
        string lastName, 
        string email,
        DateTime birthdate, 
        Gender gender, 
        List<Interests> interests,
        States state,
        DateTime createAt)
    {
        VolunteerId = volunteerId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Birthdate = birthdate;
        Gender = gender;
        Interests = interests;
        State = state;
        CreateAt = createAt;
    }

    public int VolunteerId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateTime Birthdate { get; }
    public Gender Gender { get; }
    public List<Interests> Interests { get; }
    public States State { get; }
    public DateTime CreateAt { get; }
    public string Email { get; }
}