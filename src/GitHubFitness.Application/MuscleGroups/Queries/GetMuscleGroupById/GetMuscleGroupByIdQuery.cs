using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.MuscleGroups.Queries.GetMuscleGroupById;

public record GetMuscleGroupByIdQuery(
    string Id) : IRequest<ErrorOr<MuscleGroup>>;