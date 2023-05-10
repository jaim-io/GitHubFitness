namespace SpartanFitness.Contracts.Authentication;

public record AuthenticationResponse(
  string Id,
  string FirstName,
  string LastName,
  string ProfileImage,
  string Email,
  List<string> SavedExerciseIds,
  List<string> SavedMuscleIds,
  List<string> SavedMuscleGroupIds,
  string Token,
  string RefreshToken);