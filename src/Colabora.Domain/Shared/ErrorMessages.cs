namespace Colabora.Domain.Shared;

public static class ErrorMessages
{
    private const string OrganizationAlreadyVerified = "Organization {0} is already verified";
    
    public static Error CreateOrganizationAlreadyVerified(int id) =>
        Error.Create(nameof(OrganizationAlreadyVerified), string.Format(OrganizationAlreadyVerified, id));
}