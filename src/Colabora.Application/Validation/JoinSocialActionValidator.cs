using Colabora.Application.Features.SocialAction.JoinSocialAction.Models;
using FluentValidation;

namespace Colabora.Application.Validation;

public class JoinSocialActionValidator : AbstractValidator<JoinSocialActionCommand>
{
    public JoinSocialActionValidator()
    {
        RuleFor(command => command.VolunteerId).GreaterThan(0);
        RuleFor(command => command.SocialActionId).GreaterThan(0);
    }
}