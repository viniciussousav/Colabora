using Colabora.Application.Commons;

namespace Colabora.Application.Handlers.Volunteers;

public static class ErrorMessages
{
    public static Error CreateEmailConflictErrorMessage(string email)
        => new("Email", $"Email ${email} already registered");
    
    public static Error CreateInternalErrorException(string message)
        => new("Internal", message);
}