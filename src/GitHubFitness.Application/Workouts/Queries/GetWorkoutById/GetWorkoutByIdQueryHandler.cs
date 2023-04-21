using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.Workouts.Queries.GetWorkoutById;

public class GetWorkoutByIdQueryHandler
  : IRequestHandler<GetWorkoutByIdQuery, ErrorOr<Workout>>
{
  private readonly ICoachRepository _coachRepository;
  private readonly IWorkoutRepository _workoutRepository;

  public GetWorkoutByIdQueryHandler(
    ICoachRepository coachRepository,
    IWorkoutRepository workoutRepository)
  {
    _coachRepository = coachRepository;
    _workoutRepository = workoutRepository;
  }

  public async Task<ErrorOr<Workout>> Handle(
    GetWorkoutByIdQuery query,
    CancellationToken cancellationToken)
  {
    var coachId = CoachId.Create(query.CoachId);
    var workoutId = WorkoutId.Create(query.WorkoutId);

    if (await _coachRepository.GetByIdAsync(coachId) is not Coach coach)
    {
      return Errors.Coach.NotFound;
    }

    if (await _workoutRepository.GetByIdAsync(workoutId) is not Workout workout)
    {
      return Errors.Workout.NotFound;
    }

    return workout;
  }
}