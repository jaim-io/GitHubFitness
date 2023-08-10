using FluentValidation;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedMuscleIds;

public class GetAllSavedMuscleIdsQueryValidator : AbstractValidator<GetAllSavedMuscleIdsQuery>
{
  public GetAllSavedMuscleIdsQueryValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The user ID must be a valid GUID.");
  }
}