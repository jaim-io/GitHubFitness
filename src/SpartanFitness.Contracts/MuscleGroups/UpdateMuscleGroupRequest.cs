namespace SpartanFitness.Contracts.MuscleGroups;

public record UpdateMuscleGroupRequest(
  string Id,
  string Name,
  string Description,
  List<string>? MuscleIds,
  string Image);