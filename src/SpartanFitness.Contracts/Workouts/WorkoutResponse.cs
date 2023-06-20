namespace SpartanFitness.Contracts.Workouts;

public record WorkoutResponse(
  string Id,
  string Name,
  string Description,
  string CoachId,
  string Image,
  List<string> MuscleIds,
  List<string> MuscleGroupIds,
  List<WorkoutExerciseResponse> WorkoutExercises,
  DateTime CreatedDateTime,
  DateTime UpdatedDateTime);

public record WorkoutExerciseResponse(
  string Id,
  uint OrderNumber,
  string ExerciseId,
  uint Sets,
  uint MinReps,
  uint MaxReps,
  string ExerciseType);