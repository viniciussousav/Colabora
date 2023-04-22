using Colabora.Application.Commons;
using Colabora.Infrastructure.Auth;

namespace Colabora.Application.Shared;

public static class ErrorMessages
{
    public const string CreateOrganizationConflict = "Already exist an organization with same name and email created by this user";
    public const string VolunteerEmailConflict = "Already exist an volunteer with email {0} registered";
    public const string JoinSocialActionConflict = "Volunteer is already registered";
    
    public const string VolunteerNotFound = "Volunteer not found";
    public const string OrganizationNotFound = "Organization not found";
    public const string SocialActionNotFound = "Social Action not found";

    public const string InvalidOAuthToken = "{0} OAuth Token is invalid - {1}";

    public const string InternalError = "An unexpected error happened";

    public static Error CreateOrganizationEmailAlreadyExists(string name)
        => Error.Create(nameof(CreateOrganizationConflict), CreateOrganizationConflict, name);
    
    public static Error CreateVolunteerEmailAlreadyExists(string name)
        => Error.Create(nameof(VolunteerEmailConflict), VolunteerEmailConflict, name);
    
    public static Error CreateVolunteerNotFound()
        => Error.Create(nameof(VolunteerNotFound), VolunteerNotFound);
    
    public static Error CreateOrganizationNotFound()
        => Error.Create(nameof(OrganizationNotFound), OrganizationNotFound);

    public static Error CreateInternalError(string? message)
        => Error.Create(nameof(InternalError),  message ?? "An unexpected error happened");

    public static Error CreateSocialActionNotFound()
        => Error.Create(nameof(SocialActionNotFound), SocialActionNotFound);

    public static Error CreateJoinSocialActionConflict()
        => Error.Create(nameof(JoinSocialActionConflict), JoinSocialActionConflict);
    
    public static Error CreateInvalidOAuthToken(AuthProvider authProvider, string error = "Empty token")
        => Error.Create(nameof(InvalidOAuthToken), string.Format(InvalidOAuthToken, authProvider, error));
}