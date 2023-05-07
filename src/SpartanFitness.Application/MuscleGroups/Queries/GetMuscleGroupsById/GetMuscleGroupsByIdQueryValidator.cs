using FluentValidation;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupsById;

public class GetMuscleGroupsByIdQueryValidator : AbstractValidator<GetMuscleGroupsByIdQuery>
{
  public GetMuscleGroupsByIdQueryValidator()
  {
    RuleForEach(x => x.Ids)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Muscle id must be a valid GUID");
  }
}