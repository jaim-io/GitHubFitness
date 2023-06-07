namespace SpartanFitness.Contracts.MuscleGroups;

public record MuscleGroupResponse(
    string Id,
    string Name,
    string Description,
    string Image,
    List<string> MuscleIds,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);