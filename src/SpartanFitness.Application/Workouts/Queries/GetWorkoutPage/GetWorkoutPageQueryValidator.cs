using FluentValidation;

namespace SpartanFitness.Application.Workouts.Queries.GetWorkoutPage;

public class GetWorkoutPageQueryValidator : AbstractValidator<GetWorkoutPageQuery>
{
  public GetWorkoutPageQueryValidator()
  {
    RuleFor(x => x.PageNumber)
      .Must(x => x == null || x > 0)
      .WithMessage("The page number must be bigger than 0");

    RuleFor(x => x.PageSize)
      .Must(x => x == null || x > 0)
      .WithMessage("The page size must be bigger than 0");
  }
}