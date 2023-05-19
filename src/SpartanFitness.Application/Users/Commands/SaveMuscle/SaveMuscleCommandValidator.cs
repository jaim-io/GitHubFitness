using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.SaveMuscle;

public class SaveMuscleCommandValidator : AbstractValidator<SaveMuscleCommand>
{
  public SaveMuscleCommandValidator()
  {
    RuleFor(x => x.MuscleId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Exercise ID must be a valid GUID");

    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");
  }
}