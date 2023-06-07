using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Exercises.Queries.GetAllExercises;

public class GetAllExercisesQueryHandler : IRequestHandler<GetAllExercisesQuery, ErrorOr<List<Exercise>>>
{
  private readonly IExerciseRepository _exerciseRepository;

  public GetAllExercisesQueryHandler(IExerciseRepository exerciseRepository)
  {
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<List<Exercise>>> Handle(GetAllExercisesQuery request, CancellationToken cancellationToken)
  {
    var exercises = await _exerciseRepository.GetAllAsync();

    return exercises.Count() > 0
      ? exercises.ToList()
      : new();
  }
}