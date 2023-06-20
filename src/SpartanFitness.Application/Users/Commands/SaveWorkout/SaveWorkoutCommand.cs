using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.SaveWorkout;

public record SaveWorkoutCommand(
  string UserId,
  string WorkoutId) : IRequest<ErrorOr<Unit>>;