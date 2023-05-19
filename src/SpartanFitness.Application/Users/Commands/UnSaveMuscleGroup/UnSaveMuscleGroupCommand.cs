using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscleGroup;

public record UnSaveMuscleGroupCommand(
  string UserId, string MuscleGroupId) : IRequest<ErrorOr<Unit>>;