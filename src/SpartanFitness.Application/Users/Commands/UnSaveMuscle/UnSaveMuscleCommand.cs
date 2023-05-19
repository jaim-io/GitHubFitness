using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscle;

public record UnSaveMuscleCommand(
  string UserId, string MuscleId) : IRequest<ErrorOr<Unit>>;