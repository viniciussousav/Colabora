namespace Colabora.Infrastructure.Auth.Exceptions;

[Serializable]
public class InvalidAuthProviderException : Exception
{
    public InvalidAuthProviderException() { }

    public InvalidAuthProviderException(string message)
        : base(message) { }

    public InvalidAuthProviderException(string message, Exception inner)
        : base(message, inner) { }
}