using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscleGroupRange;

public record UnSaveMuscleGroupRangeCommand(string UserId, List<string> MuscleGroupIds) : IRequest<ErrorOr<Unit>>;