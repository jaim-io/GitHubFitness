using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscleRange;

public class UnSaveMuscleRangeCommandValidator : AbstractValidator<UnSaveMuscleRangeCommand>
{
  public UnSaveMuscleRangeCommandValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");

    RuleForEach(x => x.MuscleIds)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Muscle ID must be a valid GUID");
  }
}