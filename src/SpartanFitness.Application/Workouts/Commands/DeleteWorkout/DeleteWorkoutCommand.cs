using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Workouts.Commands.DeleteWorkout;

public record DeleteWorkoutCommand(
  string CoachId,
  string WorkoutId) : IRequest<ErrorOr<Unit>>;