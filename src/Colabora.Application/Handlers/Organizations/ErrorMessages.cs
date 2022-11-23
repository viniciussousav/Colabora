using System.Net;
using Colabora.Application.Commons;

namespace Colabora.Application.Handlers.Organizations;

public static class ErrorMessages
{
    public const string UnexpectedErrorMessage = "An unexpected error happened";
    public const string EmailAlreadyRegistered = "Already exist an organization {0} created by this user";
    
    public static Error CreateUnexpectedError(string? message)
        => new(nameof(UnexpectedErrorMessage), HttpStatusCode.InternalServerError, message ?? "An unexpected error happened");

    public static Error CreateEmailAlreadyExists(string name)
        => new(nameof(EmailAlreadyRegistered), HttpStatusCode.Conflict, EmailAlreadyRegistered, name);
}