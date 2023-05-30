using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Domain.Shared.Enums;
using FluentValidation;

namespace Colabora.Application.Validation;

public class RegisterOrganizationValidator : AbstractValidator<RegisterOrganizationCommand>
{
    public RegisterOrganizationValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(command => command.Name)
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(255);

        RuleFor(command => command.Interests)
            .NotEmpty()
            .Must(x => !x.Contains(Interests.Undefined));

        RuleFor(command => command.State)
            .NotEqual(States.Undefined);
        
        RuleFor(command => command.VolunteerCreatorId)
            .GreaterThan(0);
    }
}