using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.UnSaveExerciseRange;

public class UnSaveExerciseRangeCommandValidator : AbstractValidator<UnSaveExerciseRangeCommand>
{
  public UnSaveExerciseRangeCommandValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");

    RuleForEach(x => x.ExerciseIds)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Exercise ID must be a valid GUID");
  }
}