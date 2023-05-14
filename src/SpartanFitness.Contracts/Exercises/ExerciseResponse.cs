namespace SpartanFitness.Contracts.Exercises;

public record ExerciseResponse(
    string Id,
    string Name,
    string Description,
    string CreatorId,
    string LastUpdaterId,
    string Image,
    string Video,
    List<string> MuscleGroupIds,
    List<string> MuscleIds,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);