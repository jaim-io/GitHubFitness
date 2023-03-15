using ErrorOr;

using MediatR;

using SpartanFitness.Application.MuscleGroups.Common;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;

public record GetMuscleGroupByIdQuery(
    string Id) : IRequest<ErrorOr<MuscleGroupResult>>;