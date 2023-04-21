using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.Exercises.Queries.GetExerciseList;

public class GetExerciseListQueryHandler
  : IRequestHandler<GetExerciseListQuery, ErrorOr<List<Exercise>>>
{
  private readonly IExerciseRepository _exerciseRepository;

  public GetExerciseListQueryHandler(IExerciseRepository exerciseRepository)
  {
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<List<Exercise>>> Handle(
    GetExerciseListQuery query,
    CancellationToken cancellationToken)
  {
    return await _exerciseRepository.GetAll();
  }
}