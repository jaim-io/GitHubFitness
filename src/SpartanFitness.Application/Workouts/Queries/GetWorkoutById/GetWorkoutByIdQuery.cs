using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Workouts.Queries.GetWorkoutById;

public record GetWorkoutByIdQuery(
  string CoachId,
  string WorkoutId): IRequest<ErrorOr<Workout>>;