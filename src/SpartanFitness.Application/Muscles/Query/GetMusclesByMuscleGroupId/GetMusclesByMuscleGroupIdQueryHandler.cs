using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Muscles.Query.GetMusclesByMuscleGroupId;

public class
  GetMusclesByMuscleGroupIdQueryHandler : IRequestHandler<GetMusclesByMuscleGroupIdQuery, ErrorOr<List<Muscle>>>
{
  private readonly IMuscleRepository _muscleRepository;
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public GetMusclesByMuscleGroupIdQueryHandler(
    IMuscleRepository muscleRepository,
    IMuscleGroupRepository muscleGroupRepository)
  {
    _muscleRepository = muscleRepository;
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<List<Muscle>>> Handle(GetMusclesByMuscleGroupIdQuery query, CancellationToken cancellationToken)
  {
    var ids = query.Ids.ConvertAll(MuscleGroupId.Create);

    if (!await _muscleGroupRepository.ExistsAsync(ids))
    {
      return Errors.MuscleGroup.NotFound;
    }

    var muscles = await _muscleRepository.GetByMuscleGroupIdAsync(ids);

    return muscles;
  }
}