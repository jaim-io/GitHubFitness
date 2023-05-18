using FluentValidation;

namespace SpartanFitness.Application.Exercises.Commands.UpdateExercise;

public class UpdateExerciseCommandValidator : AbstractValidator<UpdateExerciseCommand>
{
  public UpdateExerciseCommandValidator()
  {
    RuleFor(x => x.Id)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The ID must be a valid GUID");

    RuleFor(x => x.LastUpdaterId)
      .Must(x => Guid.TryParse(x, out _))
      .WithMessage("The 'LastUpdaterId' must be a valid GUID");

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

    RuleFor(x => x.Video)
      .NotEmpty();
  }
}