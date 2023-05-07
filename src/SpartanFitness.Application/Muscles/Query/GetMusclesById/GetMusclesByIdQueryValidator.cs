using FluentValidation;

namespace SpartanFitness.Application.Muscles.Query.GetMusclesById;

public class GetMusclesByIdQueryValidator : AbstractValidator<GetMusclesByIdQuery>
{
  public GetMusclesByIdQueryValidator()
  {
    RuleForEach(x => x.Ids)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Muscle id must be a valid GUID");
  } 
}