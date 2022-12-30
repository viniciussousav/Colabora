using System.Net;
using Colabora.Application.Commons;

namespace Colabora.Application.Shared;

public static class ErrorMessages
{
    public const string OrganizationEmailAlreadyRegistered = "Already exist an organization {0} created by this user";
    public const string VolunteerEmailAlreadyRegistered = "Already exist an volunteer with email {0} registered";
    
    public const string VolunteerNotFound = "Volunteer not found";
    public const string OrganizationNotFound = "Organization not found";

    public const string InternalError = "An unexpected error happened";

    public static Error CreateOrganizationEmailAlreadyExists(string name)
        => new(nameof(OrganizationEmailAlreadyRegistered), HttpStatusCode.Conflict, OrganizationEmailAlreadyRegistered, name);
    
    public static Error CreateVolunteerEmailAlreadyExists(string name)
        => new(nameof(VolunteerEmailAlreadyRegistered), HttpStatusCode.Conflict, VolunteerEmailAlreadyRegistered, name);
    
    public static Error CreateVolunteerNotFound()
        => new(nameof(VolunteerNotFound), HttpStatusCode.NotFound, VolunteerNotFound);
    
    public static Error CreateOrganizationNotFound()
        => new(nameof(OrganizationNotFound), HttpStatusCode.NotFound, OrganizationNotFound);

    public static Error CreateInternalError(string? message)
        => new(nameof(InternalError), HttpStatusCode.InternalServerError, message ?? "An unexpected error happened");
}