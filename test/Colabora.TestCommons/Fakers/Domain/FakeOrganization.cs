using Colabora.Application.Features.Volunteer.RegisterOrganization.Models;
using Colabora.Domain.Organization;

namespace Colabora.TestCommons.Fakers.Domain;

public static class FakerOrganization
{
    public static Organization Create(RegisterOrganizationCommand command)
    {
        return new Organization(
            name: command.Name,
            email: command.Email,
            state: command.State,
            interests: command.Interests,
            volunteerCreatorId: command.VolunteerCreatorId);
    }
}