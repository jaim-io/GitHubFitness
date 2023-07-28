using FluentValidation;

namespace SpartanFitness.Application.Workouts.Commands.DeleteWorkout;

public class DeleteWorkoutCommandValidator : AbstractValidator<DeleteWorkoutCommand>
{
  public DeleteWorkoutCommandValidator()
  {
    RuleFor(x => x.CoachId)
      .Must(x => Guid.TryParse(x, out Guid _))
      .WithMessage("Coach ID must be a valid GUID");
    
    RuleFor(x => x.WorkoutId)
      .Must(x => Guid.TryParse(x, out Guid _))
      .WithMessage("Workout ID must be a valid GUID");
  }
}