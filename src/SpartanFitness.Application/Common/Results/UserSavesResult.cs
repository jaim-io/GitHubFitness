using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Common.Results;

public record UserSavesResult(
  List<Muscle> Muscles,
  List<MuscleGroup> MuscleGroups,
  List<Exercise> Exercises,
  List<Workout> Workouts);