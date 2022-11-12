using Colabora.Domain.Shared.Enums;

namespace Colabora.Domain.Entities.Volunteers;

public class Volunteer
{
    public Volunteer(
        string firstName, 
        string lastName, 
        DateTime birthday, 
        Gender gender, 
        States state,
        DateTime createAt)
    {
        FirstName = firstName;
        LastName = lastName;
        Birthday = birthday;
        Gender = gender;
        State = state;
        CreateAt = createAt;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public DateTime Birthday { get; }
    public Gender Gender { get; }
    public States State { get; }
    public DateTime CreateAt { get; }
}