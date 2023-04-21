using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.MuscleGroups.Commands.CreateMuscleGroup;

public record CreateMuscleGroupCommand(
    string UserId,
    string Name,
    string Description,
    string Image) : IRequest<ErrorOr<MuscleGroup>>;