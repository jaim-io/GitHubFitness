using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.UnSaveWorkout;

public class UnSaveWorkoutCommandValidator : AbstractValidator<UnSaveWorkoutCommand>
{
  public UnSaveWorkoutCommandValidator()
  {
    RuleFor(x => x.WorkoutId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Workout ID must be a valid GUID");

    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");
  }
}