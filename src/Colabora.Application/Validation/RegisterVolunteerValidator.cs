using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Domain.Shared.Enums;
using Colabora.Domain.Volunteer;
using FluentValidation;

namespace Colabora.Application.Validation;

public class RegisterVolunteerValidator : AbstractValidator<RegisterVolunteerCommand>
{
    public RegisterVolunteerValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(command => command.FirstName)
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(255);
        
        RuleFor(command => command.LastName)
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(255);

        RuleFor(command => command.Interests)
            .NotEmpty()
            .Must(x => !x.Contains(Interests.Undefined));

        RuleFor(command => command.State)
            .NotEqual(States.Undefined);

        RuleFor(command => command.Birthdate.Year)
            .GreaterThan(1900);

        RuleFor(command => command.Gender)
            .NotEqual(Gender.Undefined);
    }
}