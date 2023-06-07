using FluentValidation;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupsByMuscleIds;

public class GetMuscleGroupsByMuscleIdsQueryValidator : AbstractValidator<GetMuscleGroupsByMuscleIdsQuery>
{
  public GetMuscleGroupsByMuscleIdsQueryValidator()
  {
    RuleForEach(x => x.MuscleIds)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The muscle ID must be a valid GUID");

    RuleFor(x => x.MuscleIds)
      .Must(x => x == null || x.Distinct().Count() == x.Count)
      .WithMessage("The list of muscle IDs has to contain unique values");
  }
}