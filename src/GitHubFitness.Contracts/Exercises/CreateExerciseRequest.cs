namespace GitHubFitness.Contracts.Exercises;

public record CreateExerciseRequest(
    string Name,
    string Description,
    List<string>? MuscleGroupIds,
    string Image,
    string Video);