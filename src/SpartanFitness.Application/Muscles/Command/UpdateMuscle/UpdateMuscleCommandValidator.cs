using FluentValidation;

namespace SpartanFitness.Application.Muscles.Command.UpdateMuscle;

public class UpdateMuscleCommandValidator : AbstractValidator<UpdateMuscleCommand>
{
  public UpdateMuscleCommandValidator()
  {
    RuleFor(x => x.Id)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The ID must be a valid GUID");

    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(100);

    RuleFor(x => x.Description)
      .NotEmpty()
      .MaximumLength(2048);

    RuleFor(x => x.Image)
      .NotEmpty()
      .MaximumLength(2048);
  }
}