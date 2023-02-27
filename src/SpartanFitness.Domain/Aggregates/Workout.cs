using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.Entities;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class Workout : Entity<WorkoutId>
{
    private List<MuscleGroupId> _muscleGroupIds = new();
    private List<WorkoutExercise> _workoutExercises = new();
    public string Name { get; private set; }
    public string Description { get; private set; }
    public CoachId CoachId { get; private set; }
    public string Image { get; private set; }
    public IReadOnlyList<MuscleGroupId> MuscleGroupIds => _muscleGroupIds.AsReadOnly();
    public IReadOnlyList<WorkoutExercise> WorkoutExercises => _workoutExercises.AsReadOnly();
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private Workout(
        WorkoutId id,
        List<MuscleGroupId> muscleGroupIds,
        List<WorkoutExercise> workoutExercises,
        string name,
        string description,
        CoachId coachId,
        string image,
        DateTime createdDateTime,
        DateTime updatedDateTime)
        : base(id)
    {
        _muscleGroupIds = muscleGroupIds;
        _workoutExercises = workoutExercises;
        Name = name;
        Description = description;
        CoachId = coachId;
        Image = image;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

#pragma warning disable CS8618
    private Workout()
    {
    }
#pragma warning restore CS8618

    public static Workout Create(
        List<MuscleGroupId> muscleGroupIds,
        List<WorkoutExercise> workoutExercises,
        string name,
        string description,
        CoachId coachId,
        string image)
    {
        return new(
            WorkoutId.CreateUnique(),
            muscleGroupIds,
            workoutExercises,
            name,
            description,
            coachId,
            image,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}