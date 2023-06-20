namespace SpartanFitness.Contracts.Users;

public record UserResponse(
  Guid Id,
  string FirstName,
  string LastName,
  string ProfileImage,
  string Email,
  List<string> SavedExerciseIds,
  List<string> SavedMuscleIds,
  List<string> SavedMuscleGroupIds,
  List<string> SavedWorkoutIds);