namespace SpartanFitness.Contracts.MuscleGroups;

public record MuscleGroupResponse(
    string Id,
    string Name,
    string Description,
    string Image,
    string CoachId,
    DateTime CreatedDateTime,
    DateTime UpdatedDateTime);