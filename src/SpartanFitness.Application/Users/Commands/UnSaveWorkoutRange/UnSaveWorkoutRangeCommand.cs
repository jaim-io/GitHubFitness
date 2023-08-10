using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.UnSaveWorkoutRange;

public record UnSaveWorkoutRangeCommand(string UserId, List<string> WorkoutIds) : IRequest<ErrorOr<Unit>>;