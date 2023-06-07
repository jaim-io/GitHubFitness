using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.MuscleGroups.Commands.UpdateMuscleGroup;

public record UpdateMuscleGroupCommand(
  string Id,
  string Name,
  string Description,
  List<string>? MuscleIds,
  string Image) : IRequest<ErrorOr<MuscleGroup>>;