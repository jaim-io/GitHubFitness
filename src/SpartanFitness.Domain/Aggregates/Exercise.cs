using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.Events;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class Exercise : AggregateRoot<ExerciseId, Guid>
{
  private List<MuscleGroupId> _muscleGroupIds;
  private List<MuscleId> _muscleIds;
  public string Name { get; private set; }
  public string Description { get; private set; }
  public CoachId CreatorId { get; private set; }
  public CoachId LastUpdaterId { get; private set; }
  public string Image { get; private set; }
  public string Video { get; private set; }
  public IReadOnlyList<MuscleGroupId> MuscleGroupIds => _muscleGroupIds.AsReadOnly();
  public IReadOnlyList<MuscleId> MuscleIds => _muscleIds.AsReadOnly();
  public DateTime CreatedDateTime { get; private set; }
  public DateTime UpdatedDateTime { get; private set; }

  private Exercise(
    ExerciseId id,
    List<MuscleGroupId> muscleGroupIds,
    List<MuscleId> muscleIds,
    string name,
    string description,
    CoachId creatorId,
    CoachId lastUpdaterId,
    string image,
    string video,
    DateTime createdDateTime,
    DateTime updatedDateTime)
    : base(id)
  {
    _muscleGroupIds = muscleGroupIds;
    _muscleIds = muscleIds;
    Name = name;
    Description = description;
    CreatorId = creatorId;
    LastUpdaterId = lastUpdaterId;
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

  public static Exercise Create(
    List<MuscleGroupId> muscleGroupIds,
    List<MuscleId> muscleIds,
    string name,
    string description,
    CoachId coachId,
    string image,
    string video)
  {
    return new(
      ExerciseId.CreateUnique(),
      muscleGroupIds,
      muscleIds,
      name,
      description,
      coachId,
      coachId,
      image,
      video,
      DateTime.UtcNow,
      DateTime.UtcNow);
  }

  public void SetName(string name)
  {
    Name = name;
  }

  public void SetDescription(string description)
  {
    Description = description;
  }

  public void SetLastUpdater(CoachId id)
  {
    LastUpdaterId = id;
  }

  public void SetMuscleGroupIds(List<MuscleGroupId> ids)
  {
    _muscleGroupIds = ids;
  }

  public void SetMuscleIds(List<MuscleId> ids)
  {
    _muscleIds = ids;
  }

  public void SetImage(string image)
  {
    Image = image;
  }

  public void SetVideo(string video)
  {
    Video = video;
  }

  public void SetUpdatedDateTime() => UpdatedDateTime = DateTime.UtcNow;
  
  public void Delete() => AddDomainEvent(new ExerciseDeleted(this));
}