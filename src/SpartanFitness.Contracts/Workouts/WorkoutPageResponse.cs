namespace SpartanFitness.Contracts.Workouts;

public record WorkoutPageResponse(
  List<WorkoutPageWorkoutResponse> Workouts,
  int PageNumber,
  int PageCount);

public record WorkoutPageWorkoutResponse(
  string Id,
  string Name,
  string Description,
  string CoachId,
  string Image,
  List<string> MuscleIds,
  List<string> MuscleGroupIds,
  List<WorkoutPageWorkoutExerciseResponse> WorkoutExercises,
  DateTime CreatedDateTime,
  DateTime UpdatedDateTime);

public record WorkoutPageWorkoutExerciseResponse(
  string Id,
  uint OrderNumber,
  string ExerciseId,
  uint Sets,
  uint MinReps,
  uint MaxReps,
  string ExerciseType);