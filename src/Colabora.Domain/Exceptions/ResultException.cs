namespace Colabora.Domain.Exceptions;

public class ResultException : Exception
{
    public ResultException()
    {
    }

    public ResultException(string message)
        : base(message)
    {
    }

    public ResultException(string message, Exception inner)
        : base(message, inner)
    {
    }
}