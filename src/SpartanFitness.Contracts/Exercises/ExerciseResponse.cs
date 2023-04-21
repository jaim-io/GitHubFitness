namespace SpartanFitness.Contracts.Exercises;

public record ExerciseResponse(
    string Id,
    string Name, 
    string Description,
    string CreatorId,
    string Image,
    string Video,
    List<string> MuscleGroupIds,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);