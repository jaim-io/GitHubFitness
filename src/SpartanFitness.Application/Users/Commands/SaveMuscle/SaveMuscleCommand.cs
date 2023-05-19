using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.SaveMuscle;

public record SaveMuscleCommand(
  string UserId, string MuscleId) : IRequest<ErrorOr<Unit>>;