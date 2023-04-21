using FluentValidation;

namespace GitHubFitness.Application.Authentication.Queries.VerifyIfUserIsCoach;

public class VerifyIfUserIsCoachQueryValidator : AbstractValidator<VerifyIfUserIsCoachQuery>
{
  public VerifyIfUserIsCoachQueryValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The workout ID must be a valid GUID");

    RuleFor(x => x.CoachId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The coach ID must be a valid GUID");
  }
}