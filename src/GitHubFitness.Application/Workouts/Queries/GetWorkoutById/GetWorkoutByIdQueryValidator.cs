using FluentValidation;

namespace GitHubFitness.Application.Workouts.Queries.GetWorkoutById;

public class GetWorkoutByIdQueryValidator
  : AbstractValidator<GetWorkoutByIdQuery>
{
  public GetWorkoutByIdQueryValidator()
  {
    RuleFor(x => x.CoachId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The coach ID must be a valid GUID");

    RuleFor(x => x.WorkoutId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The workout ID must be a valid GUID");
  }
}