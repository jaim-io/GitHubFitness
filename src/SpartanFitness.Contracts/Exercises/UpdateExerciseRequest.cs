using Microsoft.AspNetCore.Http;

namespace SpartanFitness.Contracts.Exercises;

public record UpdateExerciseRequest(
  string Id,
  string Name,
  string Description,
  List<string>? MuscleGroupIds,
  List<string>? MuscleIds,
  IFormFile? Image,
  string Video);