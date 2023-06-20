using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.Entities;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class Workout : AggregateRoot<WorkoutId, Guid>
{
  private List<MuscleId> _muscleIds = new();
  private List<MuscleGroupId> _muscleGroupIds = new();
  private List<WorkoutExercise> _workoutExercises = new();
  public string Name { get; private set; }
  public string Description { get; private set; }
  public CoachId CoachId { get; private set; }
  public string Image { get; private set; }
  public IReadOnlyList<MuscleId> MuscleIds => _muscleIds.AsReadOnly();
  public IReadOnlyList<MuscleGroupId> MuscleGroupIds => _muscleGroupIds.AsReadOnly();
  public IReadOnlyList<WorkoutExercise> WorkoutExercises => _workoutExercises.AsReadOnly();
  public DateTime CreatedDateTime { get; private set; }
  public DateTime UpdatedDateTime { get; private set; }

  private Workout(
    WorkoutId id,
    string name,
    string description,
    CoachId coachId,
    string image,
    List<MuscleId> muscleIds,
    List<MuscleGroupId> muscleGroupIds,
    List<WorkoutExercise> workoutExercises,
    DateTime createdDateTime,
    DateTime updatedDateTime)
    : base(id)
  {
    Name = name;
    Description = description;
    CoachId = coachId;
    Image = image;
    _muscleIds = muscleIds;
    _muscleGroupIds = muscleGroupIds;
    _workoutExercises = workoutExercises;
    CreatedDateTime = createdDateTime;
    UpdatedDateTime = updatedDateTime;
  }

#pragma warning disable CS8618
  private Workout()
  {
  }
#pragma warning restore CS8618

  public static Workout Create(
    string name,
    string description,
    CoachId coachId,
    string image,
    List<MuscleId> muscleIds,
    List<MuscleGroupId> muscleGroupIds,
    List<WorkoutExercise> workoutExercises)
  {
    return new(
      WorkoutId.CreateUnique(),
      name,
      description,
      coachId,
      image,
      muscleIds,
      muscleGroupIds,
      workoutExercises,
      DateTime.UtcNow,
      DateTime.UtcNow);
  }
}