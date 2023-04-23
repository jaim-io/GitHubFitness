namespace SpartanFitness.Contracts.Exercises;

public record ExercisePageResponse(
  List<ExercisePageExerciseResponse> Exercises,
  int PageNumber,
  int PageCount);

public record ExercisePageExerciseResponse(
  string Id,
  string Name,
  string Description,
  string CreatorId,
  string Image,
  string Video,
  List<string> MuscleGroupIds,
  DateTime CreatedDateTime,
  DateTime UpdatedDateTime);