using FluentValidation;

namespace SpartanFitness.Application.Muscles.Query.GetMusclePageByMuscleGroup;

public class GetMusclePageByMuscleGroupQueryValidator : AbstractValidator<GetMusclePageByMuscleGroupQuery>
{
  public GetMusclePageByMuscleGroupQueryValidator()
  {
    RuleFor(x => x.MuscleGroupId)
      .Must(id => Guid.TryParse(id, out _))
      .WithMessage("The muscle group ID must be a valid GUID");

    RuleFor(x => x.PageNumber)
      .Must(x => x == null || x > 0)
      .WithMessage("The page number must be bigger than 0");

    RuleFor(x => x.PageSize)
      .Must(x => x == null || x > 0)
      .WithMessage("The page size must be bigger than 0");
  }
}