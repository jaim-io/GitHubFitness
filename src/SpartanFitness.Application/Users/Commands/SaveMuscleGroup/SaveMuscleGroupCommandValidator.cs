using FluentValidation;

namespace SpartanFitness.Application.Users.Commands.SaveMuscleGroup;

public class SaveMuscleGroupCommandValidator : AbstractValidator<SaveMuscleGroupCommand>
{
  public SaveMuscleGroupCommandValidator()
  {
    RuleFor(x => x.MuscleGroupId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("Exercise ID must be a valid GUID");

    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("User ID must be a valid GUID");
  }
}