using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;

public record GetMuscleGroupByIdQuery(
    string Id) : IRequest<ErrorOr<MuscleGroup>>;