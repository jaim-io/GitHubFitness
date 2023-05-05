namespace SpartanFitness.Contracts.Exercises;

public record CreateExerciseRequest(
    string Name,
    string Description,
    List<string>? MuscleGroupIds,
    List<string>? MuscleIds,
    string Image,
    string Video);