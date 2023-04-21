using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Workouts.Commands.CreateWorkout;

public record CreateWorkoutCommand(
  string Name,
  string Description,
  string CoachId,
  string Image,
  List<CreateWorkoutExerciseCommand>? WorkoutExercises) : IRequest<ErrorOr<Workout>>;

public record CreateWorkoutExerciseCommand(
  string ExerciseId,
  uint OrderNumber,
  uint Sets,
  uint MinReps,
  uint MaxReps,
  string ExerciseType);