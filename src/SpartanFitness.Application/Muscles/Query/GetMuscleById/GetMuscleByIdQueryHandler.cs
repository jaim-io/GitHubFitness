using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Muscles.Query.GetMuscleById;

public class GetMuscleByIdQueryHandler : IRequestHandler<GetMuscleByIdQuery, ErrorOr<Muscle>>
{
  private readonly IMuscleRepository _muscleRepository;

  public GetMuscleByIdQueryHandler(IMuscleRepository muscleRepository)
  {
    _muscleRepository = muscleRepository;
  }

  public async Task<ErrorOr<Muscle>> Handle(GetMuscleByIdQuery query, CancellationToken cancellationToken)
  {
    var id = MuscleId.Create(query.Id);

    if (await _muscleRepository.GetByIdAsync(id) is not Muscle muscle)
    {
      return Errors.Muscle.NotFound;
    }

    return muscle;
  }
}