using System.Net;
using Colabora.Application.Commons;

namespace Colabora.Application.Shared;

public static class ErrorMessages
{
    public const string CreateOrganizationConflict = "Already exist an organization with same name and email created by this user";
    public const string VolunteerEmailConflict = "Already exist an volunteer with email {0} registered";
    public const string JoinSocialActionConflict = "Volunteer is already registered";
    
    public const string VolunteerNotFound = "Volunteer not found";
    public const string OrganizationNotFound = "Organization not found";
    public const string SocialActionNotFound = "Social Action not found";

    public const string InternalError = "An unexpected error happened";

    public static Error CreateOrganizationEmailAlreadyExists(string name)
        => new(nameof(CreateOrganizationConflict), HttpStatusCode.Conflict, CreateOrganizationConflict, name);
    
    public static Error CreateVolunteerEmailAlreadyExists(string name)
        => new(nameof(VolunteerEmailConflict), HttpStatusCode.Conflict, VolunteerEmailConflict, name);
    
    public static Error CreateVolunteerNotFound()
        => new(nameof(VolunteerNotFound), HttpStatusCode.NotFound, VolunteerNotFound);
    
    public static Error CreateOrganizationNotFound()
        => new(nameof(OrganizationNotFound), HttpStatusCode.NotFound, OrganizationNotFound);

    public static Error CreateInternalError(string? message)
        => new(nameof(InternalError), HttpStatusCode.InternalServerError, message ?? "An unexpected error happened");

    public static Error CreateSocialActionNotFound()
        => new(nameof(SocialActionNotFound), HttpStatusCode.NotFound, SocialActionNotFound);

    public static Error CreateJoinSocialActionConflict()
        => new(nameof(JoinSocialActionConflict), HttpStatusCode.Conflict, JoinSocialActionConflict);
}