using Colabora.Application.Commons;

namespace Colabora.Application.Handlers.Organizations;

public static class ErrorMessages
{
    public static Error CreateUnexpectedErrorMessage(string message)
        => new("Unexpected Error", message);

    public static Error CreateOrganizationConflict(string name)
        => new("Conflict", $"Already exist an organization {name} created by this user");
}