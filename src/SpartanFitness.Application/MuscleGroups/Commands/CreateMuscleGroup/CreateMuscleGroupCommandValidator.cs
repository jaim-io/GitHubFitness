using FluentValidation;

namespace SpartanFitness.Application.MuscleGroups.Commands.CreateMuscleGroup;

public class CreateMuscleGroupCommandValidator
  : AbstractValidator<CreateMuscleGroupCommand>
{
  public CreateMuscleGroupCommandValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(100);

    RuleFor(x => x.Description)
      .NotEmpty()
      .MaximumLength(2048);

    RuleFor(x => x.Image)
      .NotEmpty()
      .MaximumLength(2048)
      .Must(x => x.StartsWith("https://"))
      .WithMessage("Image url has to start with 'https://'.");
  }
}