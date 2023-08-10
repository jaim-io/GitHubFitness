using FluentValidation;

using SpartanFitness.Application.Users.Queries.GetSavedMusclePage;

namespace SpartanFitness.Application.Users.Queries.GetSavedWorkoutPage;

public class GetSavedWorkoutPageQueryValidator
  : AbstractValidator<GetSavedWorkoutPageQuery>
{
  public GetSavedWorkoutPageQueryValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The user ID must be a valid GUID");

    RuleFor(x => x.PageNumber)
      .Must(x => x is null or > 0)
      .WithMessage("The page number must be bigger than 0");

    RuleFor(x => x.PageSize)
      .Must(x => x is null or > 0)
      .WithMessage("The page size must be bigger than 0");
  }
}