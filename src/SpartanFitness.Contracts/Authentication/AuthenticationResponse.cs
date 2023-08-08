namespace SpartanFitness.Contracts.Authentication;

public record AuthenticationResponse(
  string Id,
  string FirstName,
  string LastName,
  string ProfileImage,
  string Email,
  List<AuthenticationResponseRole> Roles,
  List<string> SavedExerciseIds,
  List<string> SavedMuscleIds,
  List<string> SavedMuscleGroupIds,
  List<string> SavedWorkoutIds,
  DateTime CreatedDateTime,
  DateTime UpdatedDateTime,
  string Token,
  string RefreshToken);

public record AuthenticationResponseRole(
  string Name,
  string Id);