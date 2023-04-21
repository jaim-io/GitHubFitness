using FluentValidation;

using GitHubFitness.Domain.Common.Models;
using GitHubFitness.Domain.Enums;

namespace GitHubFitness.Application.Workouts.Commands.CreateWorkout;

public class CreateWorkoutExerciseCommandValidator
  : AbstractValidator<CreateWorkoutExerciseCommand>
{
  public CreateWorkoutExerciseCommandValidator()
  {
    RuleFor(x => x.ExerciseId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The exercise ID must be a valid GUID");

    RuleFor(x => x.ExerciseType)
      .Must(x => Enumeration
                  .Getall<ExerciseType>()
                  .Select(e => e.Name)
                  .Contains(x))
      .WithMessage("Exercise type must be a valid exercise type");

    RuleFor(x => x.Sets)
      .Must(x => x < 51)
      .WithMessage("Sets must be greater than 0 and less than 51");

    RuleFor(x => x.MinReps)
      .Must(x => x < 51)
      .WithMessage("Minimal reps must be greater than 0 and less than 51");

    RuleFor(x => x.MaxReps)
     .Must(x => x < 51)
     .WithMessage("Maximal reps must be greater than 0 and less than 51");

    RuleFor(x => x)
      .Must(x => x.MinReps <= x.MaxReps)
      .WithMessage("Minimal reps must be less than or equal to maximal reps");
  }
}