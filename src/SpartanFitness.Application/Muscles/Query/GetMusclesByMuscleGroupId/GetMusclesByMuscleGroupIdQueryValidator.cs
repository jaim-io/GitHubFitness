using FluentValidation;

namespace SpartanFitness.Application.Muscles.Query.GetMusclesByMuscleGroupId;

public class GetMusclesByMuscleGroupIdQueryValidator : AbstractValidator<GetMusclesByMuscleGroupIdQuery>
{
  public GetMusclesByMuscleGroupIdQueryValidator()
  {
    RuleForEach(x => x.Ids)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Muscle group id must be a valid GUID");
  }
}