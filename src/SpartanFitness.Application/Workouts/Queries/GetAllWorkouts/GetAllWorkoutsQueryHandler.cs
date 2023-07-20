using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Workouts.Queries.GetAllWorkouts;

public class GetAllWorkoutsQueryHandler : IRequestHandler<GetAllWorkoutsQuery, ErrorOr<List<Workout>>>
{
  private readonly IWorkoutRepository _workoutRepository;

  public GetAllWorkoutsQueryHandler(IWorkoutRepository workoutRepository)
  {
    _workoutRepository = workoutRepository;
  }

  public async Task<ErrorOr<List<Workout>>> Handle(GetAllWorkoutsQuery query, CancellationToken cancellationToken)
  {
    var workouts = await _workoutRepository.GetAllAsync();
    return workouts.ToList();
  }
}