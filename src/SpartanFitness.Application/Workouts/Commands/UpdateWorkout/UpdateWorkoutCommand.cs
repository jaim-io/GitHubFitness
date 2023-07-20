using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Workouts.Commands.UpdateWorkout;

public record UpdateWorkoutCommand(
  string Id,
  string CoachId,
  string Name,
  string Description,
  string Image,
  List<UpdateWorkoutExerciseCommand>? WorkoutExercises) : IRequest<ErrorOr<Workout>>;

public record UpdateWorkoutExerciseCommand(
  string Id,
  string ExerciseId,
  uint OrderNumber,
  uint Sets,
  uint MinReps,
  uint MaxReps,
  string ExerciseType);