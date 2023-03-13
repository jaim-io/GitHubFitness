using ErrorOr;

using MediatR;

using SpartanFitness.Application.MuscleGroups.Common;

namespace SpartanFitness.Application.MuscleGroups.CreateMuscleGroup;

public record CreateMuscleGroupCommand(
    string UserId,
    string Name,
    string Description,
    string Image) : IRequest<ErrorOr<MuscleGroupResult>>;