using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetAllMuscleGroups;

public class GetAllMuscleGroupsQueryHandler : IRequestHandler<GetAllMuscleGroupsQuery, ErrorOr<List<MuscleGroup>>>
{
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public GetAllMuscleGroupsQueryHandler(IMuscleGroupRepository muscleGroupRepository)
  {
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<List<MuscleGroup>>> Handle(GetAllMuscleGroupsQuery request, CancellationToken cancellationToken)
  {
    var muscles = await _muscleGroupRepository.GetAllAsync();
    return muscles.ToList();
  }
}