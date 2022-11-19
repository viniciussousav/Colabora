using Colabora.Application.Handlers.Organizations.RegisterOrganization.Models;
using Colabora.Domain.Entities;

namespace Colabora.Application.Handlers.Organizations.RegisterOrganization.Mappers;

public static class RegisterOrganizationCommandMapper
{
    public static Organization MapToOrganization(this RegisterOrganizationCommand command)
    {
        return new Organization(
            default,
            command.Name,
            command.Email,
            command.State,
            command.Interests,
            command.Memberships,
            command.CreatedBy,
            DateTime.Now);
    } 
}