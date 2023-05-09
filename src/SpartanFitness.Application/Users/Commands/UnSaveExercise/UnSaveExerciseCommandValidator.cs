using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.UnSaveExercise;

public class UnSaveExerciseCommandValidator : AbstractValidator<UnSaveExerciseCommand>
{
  public UnSaveExerciseCommandValidator()
  {
    RuleFor(x => x.ExerciseId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Exercise ID must be a valid GUID");

    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");
  }
}