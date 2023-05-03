using FluentValidation;

namespace SpartanFitness.Application.Muscles.Command.CreateMuscle;

public class CreateMuscleCommandValidator : AbstractValidator<CreateMuscleCommand>
{
  public CreateMuscleCommandValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty();

    RuleFor(x => x.Description)
      .NotEmpty();

    RuleFor(x => x.Image)
      .NotEmpty();

    RuleFor(x => x.MuscleGroupId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The muscle group ID must be a valid GUID");
  }
}