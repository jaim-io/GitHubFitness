using FluentValidation;

namespace SpartanFitness.Application.Exercises.Commands.CreateExercise;

public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
  public CreateExerciseCommandValidator()
  {
    RuleFor(x => x.UserId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The user ID must be a valid GUID");

    RuleForEach(x => x.MuscleGroupIds)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The muscle group ID must be a valid GUID");

    RuleFor(x => x.MuscleGroupIds)
      .Must(x => x == null || x.Distinct().Count() == x.Count)
      .WithMessage("The list of muscle group IDs has to contain unique values");

    RuleForEach(x => x.MuscleIds)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The muscle group ID must be a valid GUID");

    RuleFor(x => x.MuscleIds)
      .Must(x => x == null || x.Distinct().Count() == x.Count)
      .WithMessage("The list of muscle group IDs has to contain unique values");

    RuleFor(x => x.Name)
      .NotEmpty()
      .MaximumLength(100);

    RuleFor(x => x.Description)
      .NotEmpty()
      .MaximumLength(2048);

    RuleFor(x => x.Image)
      .NotEmpty()
      .MaximumLength(2048);

    RuleFor(x => x.Video)
      .NotEmpty()
      .MaximumLength(2048);
  }
}