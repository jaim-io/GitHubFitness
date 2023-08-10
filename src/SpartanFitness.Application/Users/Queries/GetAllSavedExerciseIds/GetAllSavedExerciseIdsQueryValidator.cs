using FluentValidation;

namespace SpartanFitness.Application.Users.Queries.GetAllSavedExerciseIds;

public class GetAllSavedExerciseIdsQueryValidator : AbstractValidator<GetAllSavedExerciseIdsQuery>
{
  public GetAllSavedExerciseIdsQueryValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The user ID must be a valid guid.");
  }
}