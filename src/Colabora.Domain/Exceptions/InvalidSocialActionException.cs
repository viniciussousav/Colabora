namespace Colabora.Domain.Exceptions;

[Serializable]
public class InvalidSocialActionException : Exception
{
    public InvalidSocialActionException() { }

    public InvalidSocialActionException(string message)
        : base(message) { }

    public InvalidSocialActionException(string message, Exception inner)
        : base(message, inner) { }
}