using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.MuscleGroups.Commands.CreateMuscleGroup;

public record CreateMuscleGroupCommand(
    string UserId,
    string Name,
    string Description,
    string Image) : IRequest<ErrorOr<MuscleGroup>>;