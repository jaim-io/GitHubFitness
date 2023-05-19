using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscle;

public class UnSaveMuscleValidator : AbstractValidator<UnSaveMuscleCommand>
{
  public UnSaveMuscleValidator()
  {
    RuleFor(x => x.MuscleId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Exercise ID must be a valid GUID");

    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");
  }
}