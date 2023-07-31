namespace SpartanFitness.Contracts.Muscles;

public record CreateMuscleRequest(
  string Name,
  string MuscleGroupId,
  string Description,
  string Image);