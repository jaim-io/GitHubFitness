using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.Workouts.Queries.GetWorkoutById;

public record GetWorkoutByIdQuery(
  string CoachId,
  string WorkoutId) : IRequest<ErrorOr<Workout>>;