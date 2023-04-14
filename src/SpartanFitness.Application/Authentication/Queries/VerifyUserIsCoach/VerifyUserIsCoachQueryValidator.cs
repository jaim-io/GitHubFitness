using FluentValidation;

namespace SpartanFitness.Application.Authentication.Queries.VerifyUserIsCoach;

public class VerifyUserIsCoachQueryValidator : AbstractValidator<VerifyUserIsCoachQuery>
{
  public VerifyUserIsCoachQueryValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The workout ID must be a valid GUID");

    RuleFor(x => x.CoachId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The coach ID must be a valid GUID");
  }
}