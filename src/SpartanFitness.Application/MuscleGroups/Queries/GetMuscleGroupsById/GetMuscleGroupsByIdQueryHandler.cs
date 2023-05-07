using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupsById;

public class GetMuscleGroupsByIdQueryHandler : IRequestHandler<GetMuscleGroupsByIdQuery, ErrorOr<List<MuscleGroup>>>
{
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public GetMuscleGroupsByIdQueryHandler(IMuscleGroupRepository muscleGroupRepository)
  {
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<List<MuscleGroup>>> Handle(GetMuscleGroupsByIdQuery query, CancellationToken cancellationToken)
  {
    var ids = query.Ids.ConvertAll(MuscleGroupId.Create);

    if (!await _muscleGroupRepository.ExistsAsync(ids))
    {
      return Errors.MuscleGroup.NotFound;
    }

    var muscleGroups = await _muscleGroupRepository.GetByIdAsync(ids);

    return muscleGroups;
  }
}