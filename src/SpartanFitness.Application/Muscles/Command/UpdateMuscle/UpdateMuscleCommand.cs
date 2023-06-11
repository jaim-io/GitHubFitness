using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Muscles.Command.UpdateMuscle;

public record UpdateMuscleCommand(
  string Id,
  string Name,
  string Description,
  string Image) : IRequest<ErrorOr<Muscle>>;