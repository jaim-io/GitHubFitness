using FluentValidation;

namespace SpartanFitness.Application.CoachApplications.Commands.CreateCoachApplication;

public class CreateCoachApplicationCommandValidator
    : AbstractValidator<CreateCoachApplicationCommand>
{
    public CreateCoachApplicationCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The string must contain a valid GUID");

        RuleFor(x => x.Motivation)
            .NotEmpty();
    }
}