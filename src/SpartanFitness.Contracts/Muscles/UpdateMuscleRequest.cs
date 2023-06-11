namespace SpartanFitness.Contracts.Muscles;

public record UpdateMuscleRequest(
  string Id,
  string Name, 
  string Description,
  string Image);