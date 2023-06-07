using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Muscles.Query.GetAllMuscles;

public class GetAllMusclesQueryHandler : IRequestHandler<GetAllMusclesQuery, ErrorOr<List<Muscle>>>
{
  private readonly IMuscleRepository _muscleRepository;

  public GetAllMusclesQueryHandler(IMuscleRepository muscleRepository)
  {
    _muscleRepository = muscleRepository;
  }

  public async Task<ErrorOr<List<Muscle>>> Handle(GetAllMusclesQuery request, CancellationToken cancellationToken)
  {
    var muscles = await _muscleRepository.GetAllAsync();
    return muscles.Count() > 0
      ? muscles.ToList()
      : new();
  }
}