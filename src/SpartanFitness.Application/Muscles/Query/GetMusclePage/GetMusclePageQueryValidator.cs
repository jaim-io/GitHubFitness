using FluentValidation;

namespace SpartanFitness.Application.Muscles.Query.GetMusclePage;

public class GetMusclePageQueryValidator : AbstractValidator<GetMusclePageQuery>
{
  public GetMusclePageQueryValidator()
  {
    RuleFor(x => x.PageNumber)
      .Must(x => x == null || x > 0)
      .WithMessage("The page number must be bigger than 0");

    RuleFor(x => x.PageSize)
      .Must(x => x == null || x > 0)
      .WithMessage("The page size must be bigger than 0");
  }
}