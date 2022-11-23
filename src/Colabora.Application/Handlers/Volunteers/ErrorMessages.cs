using System.Net;
using Colabora.Application.Commons;

namespace Colabora.Application.Handlers.Volunteers;

public static class ErrorMessages
{
    public const string UnexpectedErrorMessage = "An unexpected error happened";
    public const string EmailAlreadyRegistered = "Already exist an volunteer with email {0} registered";
    
    public static Error CreateUnexpectedError(string? message)
        => new(nameof(UnexpectedErrorMessage), HttpStatusCode.InternalServerError, message ?? "An unexpected error happened");
    
    public static Error CreateEmailAlreadyExists(string name)
        => new(nameof(EmailAlreadyRegistered), HttpStatusCode.Conflict, EmailAlreadyRegistered, name);
}