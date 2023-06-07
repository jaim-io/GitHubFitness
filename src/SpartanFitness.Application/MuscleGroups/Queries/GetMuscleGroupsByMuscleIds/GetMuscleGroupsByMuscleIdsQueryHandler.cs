using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupsByMuscleIds;

public class GetMuscleGroupsByMuscleIdsQueryHandler : IRequestHandler<GetMuscleGroupsByMuscleIdsQuery, ErrorOr<List<MuscleGroup>>>
{
  private readonly IMuscleRepository _muscleRepository;
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public GetMuscleGroupsByMuscleIdsQueryHandler(IMuscleRepository muscleRepository, IMuscleGroupRepository muscleGroupRepository)
  {
    _muscleRepository = muscleRepository;
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<List<MuscleGroup>>> Handle(GetMuscleGroupsByMuscleIdsQuery query, CancellationToken cancellationToken)
  {
    var muscleIds = query.MuscleIds.ConvertAll(id => MuscleId.Create(id));

    if (!await _muscleRepository.ExistsAsync(muscleIds))
    {
      return Errors.Muscle.NotFound;
    }

    var muscleGroups = await _muscleGroupRepository.GetByMuscleIdAsync(muscleIds);

    return muscleGroups;
  }
}