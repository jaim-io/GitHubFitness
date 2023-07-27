using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Workouts.Commands.DeleteWorkout;

public class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand, ErrorOr<Unit>>
{
  private readonly ICoachRepository _coachRepository;
  private readonly IWorkoutRepository _workoutRepository;

  public DeleteWorkoutCommandHandler(
    ICoachRepository coachRepository,
    IWorkoutRepository workoutRepository)
  {
    _coachRepository = coachRepository;
    _workoutRepository = workoutRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(DeleteWorkoutCommand command, CancellationToken cancellationToken)
  {
    var coachId = CoachId.Create(command.CoachId);
    if (await _coachRepository.GetByIdAsync(coachId) is null)
    {
      return Errors.Coach.NotFound;
    }

    var workoutId = WorkoutId.Create(command.WorkoutId);
    if (await _workoutRepository.GetByIdAsync(workoutId) is not Workout workout)
    {
      return Errors.Workout.NotFound;
    }

    workout.Delete();
    await _workoutRepository.RemoveAsync(workout);

    return Unit.Value;
  }
}