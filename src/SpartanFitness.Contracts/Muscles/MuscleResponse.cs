namespace SpartanFitness.Contracts.Muscles;

public record MuscleResponse(
  string Id,
  string Name,
  string Description,
  string MuscleGroupId,
  string Image,
  DateTime CreatedDateTime,
  DateTime UpdatedDateTime);