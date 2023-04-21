namespace GitHubFitness.Contracts.MuscleGroups;

public record CreateMuscleGroupRequest(
    string Name,
    string Description,
    string Image);