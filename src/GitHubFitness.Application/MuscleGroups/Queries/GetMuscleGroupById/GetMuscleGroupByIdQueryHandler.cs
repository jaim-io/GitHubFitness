using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;

public class GetMuscleGroupByIdQueryHandler 
    : IRequestHandler<GetMuscleGroupByIdQuery, ErrorOr<MuscleGroup>>
{
    private readonly IMuscleGroupRepository _muscleGroupRepository;

    public GetMuscleGroupByIdQueryHandler(IMuscleGroupRepository muscleGroupRepository)
    {
        _muscleGroupRepository = muscleGroupRepository;
    }

    public async Task<ErrorOr<MuscleGroup>> Handle(
        GetMuscleGroupByIdQuery request,
        CancellationToken cancellationToken)
    {
        var muscleGroupId = MuscleGroupId.Create(request.Id);

        if (await _muscleGroupRepository.GetByIdAsync(muscleGroupId) is not MuscleGroup muscleGroup)
        {
            return Errors.MuscleGroup.NotFound;
        }

        return muscleGroup;
    }
}