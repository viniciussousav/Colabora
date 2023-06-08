namespace Colabora.Domain.Shared;

[Serializable]
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
        Error = Error.Create("DomainError", message);
    }
    
    public DomainException(Error error) : base(error.Message)
    {
        Error = error;
    }

    public DomainException(Error error, Exception inner)
        : base(error.Message, inner)
    {
        Error = error;
    }

    public Error Error { get; }
}