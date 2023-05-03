namespace SpartanFitness.Contracts.Muscles;

public record CreateMuscleRequest(
  string Name,
  string Description,
  string Image,
  string MuscleGroupId);