using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Muscles.Query.GetMusclesById;

public class GetMusclesByIdQueryHandler : IRequestHandler<GetMusclesByIdQuery, ErrorOr<List<Muscle>>>
{
  private readonly IMuscleRepository _muscleRepository;

  public GetMusclesByIdQueryHandler(IMuscleRepository muscleRepository)
  {
    _muscleRepository = muscleRepository;
  }

  public async Task<ErrorOr<List<Muscle>>> Handle(GetMusclesByIdQuery query, CancellationToken cancellationToken)
  {
    var ids = query.Ids.ConvertAll(MuscleId.Create);

    if (!await _muscleRepository.ExistsAsync(ids))
    {
      return Errors.Muscle.NotFound;
    }

    var muscles = await _muscleRepository.GetByIdAsync(ids);

    return muscles;
  }
}