using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.UnSaveWorkout;

public record UnSaveWorkoutCommand(
  string UserId,
  string WorkoutId) : IRequest<ErrorOr<Unit>>;