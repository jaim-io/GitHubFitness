using FluentValidation;

namespace SpartanFitness.Application.Muscles.Command.CreateMuscle;

public class CreateMuscleCommandValidator : AbstractValidator<CreateMuscleCommand>
{
  public CreateMuscleCommandValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(100);

    RuleFor(x => x.Description)
      .NotEmpty()
      .MaximumLength(2048);

    RuleFor(x => x.Image)
      .NotEmpty()
      .MaximumLength(2048);

    RuleFor(x => x.MuscleGroupId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The muscle group ID must be a valid GUID");
  }
}