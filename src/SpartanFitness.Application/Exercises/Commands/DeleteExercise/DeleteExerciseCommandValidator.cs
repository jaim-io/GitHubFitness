using FluentValidation;

namespace SpartanFitness.Application.Exercises.Commands.DeleteExercise;

public class DeleteExerciseCommandValidator : AbstractValidator<DeleteExerciseCommand>
{
  public DeleteExerciseCommandValidator()
  {
    RuleFor(x => x)
      .Must(x => x.AdminId is not null || x.CoachId is not null)
      .WithMessage("An admin ID and/or a coach ID has to be provided");

    RuleFor(x => x.CoachId)
      .Must(x => x is null || Guid.TryParse(x, out Guid _))
      .WithMessage("Coach ID must be a valid GUID");

    RuleFor(x => x.AdminId)
      .Must(x => x is null || Guid.TryParse(x, out Guid _))
      .WithMessage("Coach ID must be a valid GUID");

    RuleFor(x => x.ExerciseId)
      .Must(x => Guid.TryParse(x, out Guid _))
      .WithMessage("Exercise ID must be a valid GUID");
  }
}