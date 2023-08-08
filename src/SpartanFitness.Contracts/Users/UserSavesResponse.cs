using SpartanFitness.Contracts.Exercises;
using SpartanFitness.Contracts.MuscleGroups;
using SpartanFitness.Contracts.Muscles;
using SpartanFitness.Contracts.Workouts;

namespace SpartanFitness.Contracts.Users;

public record UserSavesResponse(
  List<MuscleResponse> Muscles,
  List<MuscleGroupResponse> MuscleGroups,
  List<ExerciseResponse> Exercises,
  List<WorkoutResponse> Workouts);