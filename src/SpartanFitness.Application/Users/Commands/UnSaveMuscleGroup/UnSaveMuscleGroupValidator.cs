using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscleGroup;

public class UnSaveMuscleGroupValidator : AbstractValidator<UnSaveMuscleGroupCommand>
{
  public UnSaveMuscleGroupValidator()
  {
    RuleFor(x => x.MuscleGroupId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Exercise ID must be a valid GUID");

    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");
  }
}