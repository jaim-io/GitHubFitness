namespace SpartanFitness.Contracts.Muscles;

public record MusclePageResponse(
  List<MusclePageMuscleResponse> Muscles,
  int PageNumber,
  int PageCount);

public record MusclePageMuscleResponse(
  string Id,
  string Name,
  string Description,
  string MuscleGroupId,
  string Image,
  DateTime CreatedDateTime,
  DateTime UpdatedDateTime);