namespace Colabora.Domain.Entities;

public class Membership
{
    public Membership(int volunteerId, DateTime initialDate, DateTime finishDate)
    {
        VolunteerId = volunteerId;
        InitialDate = initialDate;
        FinishDate = finishDate;
    }

    public int VolunteerId { get; }
    public DateTime InitialDate { get; }
    public DateTime FinishDate { get; }
}