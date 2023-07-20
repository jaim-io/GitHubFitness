namespace SpartanFitness.Contracts.Workouts;

public record UpdateWorkoutRequest(
  string Id,
  string Name,
  string Description,
  string Image,
  List<UpdateWorkoutExerciseRequest>? WorkoutExercises);

public record UpdateWorkoutExerciseRequest(
  string Id,
  string ExerciseId,
  uint OrderNumber,
  uint Sets,
  uint MinReps,
  uint MaxReps,
  string ExerciseType);