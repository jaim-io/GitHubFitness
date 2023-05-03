using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Muscles.Command.CreateMuscle;

public record CreateMuscleCommand(
  string Name,
  string Description,
  string Image,
  string MuscleGroupId) : IRequest<ErrorOr<Muscle>>;