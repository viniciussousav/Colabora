using Colabora.Domain.Enums;
using Colabora.Domain.ValueObjects;

#pragma warning disable CS8618

namespace Colabora.Domain.Entities;

public class Volunteer
{
    public static readonly Volunteer None = new();

    private Volunteer() { }
    
    public Volunteer(
        string firstName, 
        string lastName, 
        string email,
        DateTime birthdate, 
        Gender gender, 
        List<Interests> interests,
        States state)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Birthdate = birthdate;
        Gender = gender;
        Interests = interests;
        State = state;
    }

    public int VolunteerId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; }
    public DateTime Birthdate { get; }
    public Gender Gender { get; }
    public List<Interests> Interests { get; }
    public States State { get; }
    public List<Participation> Participations { get; }
    public DateTime CreateAt { get; }
}