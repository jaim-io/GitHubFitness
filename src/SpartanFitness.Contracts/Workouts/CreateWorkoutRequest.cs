namespace SpartanFitness.Contracts.Workouts;

public record CreateWorkoutRequest(
  string Name,
  string Description,
  string Image,
  List<WorkoutExerciseRequest>? WorkoutExercises);

public record WorkoutExerciseRequest(
  string ExerciseId,
  uint OrderNumber,
  uint Sets,
  uint MinReps,
  uint MaxReps,
  string ExerciseType);