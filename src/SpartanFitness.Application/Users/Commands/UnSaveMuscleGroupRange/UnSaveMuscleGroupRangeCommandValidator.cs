using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscleGroupRange;

public class UnSaveMuscleGroupRangeCommandValidator : AbstractValidator<UnSaveMuscleGroupRangeCommand>
{
  public UnSaveMuscleGroupRangeCommandValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");

    RuleForEach(x => x.MuscleGroupIds)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Muscle group ID must be a valid GUID");
  }
}