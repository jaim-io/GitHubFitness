using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class Exercise : Entity<ExerciseId>
{
    private List<MuscleGroupId> _muscleGroupIds;
    public string Name { get; private set; }
    public string Description { get; private set; }
    public CoachId CoachId { get; private set; }
    public string Image { get; private set; }
    public string Video { get; private set; }
    public IReadOnlyList<MuscleGroupId> MuscleGroupIds => _muscleGroupIds.AsReadOnly();

    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private Exercise(
        ExerciseId id,
        List<MuscleGroupId> muscleGroupIds,
        string name,
        string description,
        CoachId coachId,
        string image,
        string video,
        DateTime createdDateTime,
        DateTime updatedDateTime)
        : base(id)
    {
        _muscleGroupIds = muscleGroupIds;
        Name = name;
        Description = description;
        CoachId = coachId;
        Image = image;
        Video = video;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

#pragma warning disable CS8618
    private Exercise()
    {
    }
#pragma warning restore CS8618

    public Exercise Create(
        List<MuscleGroupId> muscleGroupIds,
        string name,
        string description,
        CoachId coachId,
        string image,
        string video)
    {
        return new(
            ExerciseId.CreateUnique(),
            muscleGroupIds,
            name,
            description,
            coachId,
            image,
            video,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}