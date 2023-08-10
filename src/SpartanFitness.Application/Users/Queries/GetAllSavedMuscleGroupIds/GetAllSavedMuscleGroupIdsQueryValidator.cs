using FluentValidation;

using SpartanFitness.Application.Users.Queries.GetAllSavedExerciseIds;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedMuscleGroupIds;

public class GetAllSavedMuscleGroupIdsQueryValidator : AbstractValidator<GetAllSavedMuscleGroupIdsQuery>
{
  public GetAllSavedMuscleGroupIdsQueryValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The user ID must be a valid GUID.");
  }
}