using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.MuscleGroups.Common;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;

public class GetMuscleGroupByIdQueryHandler 
    : IRequestHandler<GetMuscleGroupByIdQuery, ErrorOr<MuscleGroupResult>>
{
    private readonly IMuscleGroupRepository _muscleGroupRepository;

    public GetMuscleGroupByIdQueryHandler(IMuscleGroupRepository muscleGroupRepository)
    {
        _muscleGroupRepository = muscleGroupRepository;
    }

    public async Task<ErrorOr<MuscleGroupResult>> Handle(
        GetMuscleGroupByIdQuery request,
        CancellationToken cancellationToken)
    {
        var muscleGroupId = MuscleGroupId.Create(request.Id);

        if (await _muscleGroupRepository.GetByIdAsync(muscleGroupId) is not MuscleGroup muscleGroup)
        {
            return Errors.MuscleGroup.NotFound;
        }

        return new MuscleGroupResult(muscleGroup);
    }
}