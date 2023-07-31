using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Exercises.Commands.DeleteExercise;

public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, ErrorOr<Unit>>
{
  private readonly IAdministratorRepository _administratorRepository;
  private readonly ICoachRepository _coachRepository;
  private readonly IExerciseRepository _exerciseRepository;

  public DeleteExerciseCommandHandler(
    IAdministratorRepository administratorRepository,
    ICoachRepository coachRepository,
    IExerciseRepository exerciseRepository)
  {
    _administratorRepository = administratorRepository;
    _coachRepository = coachRepository;
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(DeleteExerciseCommand command, CancellationToken cancellationToken)
  {
    var isAuthorized = false;
    if (command.AdminId is not null)
    {
      var adminId = AdministratorId.Create(command.AdminId);
      isAuthorized = await _administratorRepository.GetByIdAsync(adminId) is not null;
    }

    if (!isAuthorized && command.CoachId is not null)
    {
      var coachId = CoachId.Create(command.CoachId);
      isAuthorized = await _coachRepository.GetByIdAsync(coachId) is not null;
    }

    if (!isAuthorized)
    {
      return Errors.Authorization.UnAuthorized;
    }

    var exerciseId = ExerciseId.Create(command.ExerciseId);
    if (await _exerciseRepository.GetByIdAsync(exerciseId) is not Exercise exercise)
    {
      return Errors.Exercise.NotFound;
    }

    exercise.Delete();
    await _exerciseRepository.RemoveAsync(exercise);

    return Unit.Value;
  }
}