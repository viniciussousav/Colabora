using Colabora.Domain.Shared;
using Colabora.Infrastructure.Auth;

namespace Colabora.Application.Shared;

public static class ErrorMessages
{
    private const string CreateOrganizationConflict = "Already exist an organization with same name and email created by this user";
    private const string VolunteerEmailConflict = "Already exist an volunteer with email {0} registered";
    private const string JoinSocialActionConflict = "Volunteer is already registered";
    
    private const string VolunteerNotFound = "Volunteer not found";
    private const string OrganizationNotFound = "Organization not found";
    private const string SocialActionNotFound = "Social Action not found";
    private const string EmailVerificationNotFound = "Email verification not found";
    
    private const string InvalidOAuthToken = "{0} OAuth Token is invalid - {1}";
    private const string EmailVerificationExpired = "Email verification is expired";
    private const string InvalidEmail = "Email \"{0}\" is invalid";
    
    private const string InternalError = "An unexpected error happened";

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

    public static Error CreateEmailVerificationNotFound()
        => Error.Create(nameof(EmailVerificationNotFound), EmailVerificationNotFound);
    
    public static Error CreateEmailVerificationExpired()
        => Error.Create(nameof(EmailVerificationExpired), EmailVerificationExpired);

    public static Error CreateInvalidEmail(string email)
        => Error.Create(nameof(InvalidEmail), InvalidEmail, email);
}