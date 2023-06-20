using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.SaveWorkout;

public class SaveWorkoutCommandValidator : AbstractValidator<SaveWorkoutCommand>
{
  public SaveWorkoutCommandValidator()
  {
    RuleFor(x => x.WorkoutId)
     .Must(x => Guid.TryParse(x, out _))
     .WithMessage("Workout ID must be a valid GUID");

    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");
  }
}