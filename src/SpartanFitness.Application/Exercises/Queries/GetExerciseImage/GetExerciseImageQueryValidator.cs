using FluentValidation;

namespace SpartanFitness.Application.Exercises.Queries.GetExerciseImage;

public class GetExerciseImageQueryValidator : AbstractValidator<GetExerciseImageQuery>
{
  public GetExerciseImageQueryValidator()
  {
    RuleFor(x => x.ExerciseId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Exercise Id must be a valid guid");
  }
}