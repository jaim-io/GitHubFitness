using FluentValidation;

namespace SpartanFitness.Application.CoachApplications.Commands.ApproveCoachApplication;

public class ApproveCoachApplicationCommandValidator :
    AbstractValidator<ApproveCoachApplicationCommand>
{
    public ApproveCoachApplicationCommandValidator() {
        RuleFor(x => x.Id)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The coach-application ID must be a valid GUID");

        RuleFor(x => x.UserId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("The user ID must be a valid GUID");

        RuleFor(x => x.Remarks)
            .NotEmpty();
    }
}