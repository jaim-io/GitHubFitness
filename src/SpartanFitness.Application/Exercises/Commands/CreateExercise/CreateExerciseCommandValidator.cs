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
      .NotEmpty();

    RuleFor(x => x.Description)
      .NotEmpty();

    RuleFor(x => x.Image)
      .NotEmpty();

    RuleFor(x => x.Video)
      .NotEmpty();
  }
}