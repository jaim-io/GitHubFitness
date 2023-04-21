using FluentValidation;

namespace GitHubFitness.Application.CoachApplications.Commands.CreateCoachApplication;

public class CreateCoachApplicationCommandValidator
    : AbstractValidator<CreateCoachApplicationCommand>
{
    public CreateCoachApplicationCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The user ID must be a valid GUID");

        RuleFor(x => x.Motivation)
            .NotEmpty();
    }
}