using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.SaveMuscleGroup;

public record SaveMuscleGroupCommand(
  string UserId, string MuscleGroupId) : IRequest<ErrorOr<Unit>>;