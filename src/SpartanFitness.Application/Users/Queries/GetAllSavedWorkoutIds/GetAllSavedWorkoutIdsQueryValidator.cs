using FluentValidation;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedWorkoutIds;

public class GetAllSavedWorkoutIdsQueryValidator : AbstractValidator<GetAllSavedWorkoutIdsQuery>
{
  public GetAllSavedWorkoutIdsQueryValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The user ID must be a valid GUID.");
  }
}