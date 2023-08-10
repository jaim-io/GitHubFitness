using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.UnSaveWorkoutRange;

public class UnSaveWorkoutRangeCommandValidator : AbstractValidator<UnSaveWorkoutRangeCommand>
{
  public UnSaveWorkoutRangeCommandValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");

    RuleForEach(x => x.WorkoutIds)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Workout ID must be a valid GUID");
  }
}