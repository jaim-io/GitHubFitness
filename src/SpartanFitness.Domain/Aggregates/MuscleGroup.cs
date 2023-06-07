using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class MuscleGroup : AggregateRoot<MuscleGroupId, Guid>
{
  private List<MuscleId> _muscleIds = new();
  public string Name { get; private set; }
  public string Description { get; private set; }
  public string Image { get; private set; }
  public IReadOnlyList<MuscleId> MuscleIds => _muscleIds.AsReadOnly();
  public DateTime CreatedDateTime { get; private set; }
  public DateTime UpdatedDateTime { get; private set; }

  private MuscleGroup(
    MuscleGroupId id,
    string name,
    string description,
    string image,
    List<MuscleId> muscleIds,
    DateTime createdDateTime,
    DateTime updatedDateTime)
    : base(id)
  {
    Name = name;
    Description = description;
    Image = image;
    _muscleIds = muscleIds;
    CreatedDateTime = createdDateTime;
    UpdatedDateTime = updatedDateTime;
  }

#pragma warning disable CS8618
  private MuscleGroup()
  {
  }
#pragma warning restore CS8618

  public static MuscleGroup Create(
    string name,
    string description,
    string image,
    List<MuscleId> muscleIds)
  {
    return new(
      MuscleGroupId.CreateUnique(),
      name,
      description,
      image,
      muscleIds,
      DateTime.UtcNow,
      DateTime.UtcNow);
  }

  public void SetName(string name) => Name = name;
  public void SetDescription(string description) => Description = description;
  public void SetMuscleIds(List<MuscleId> muscleIds) => _muscleIds = muscleIds;
  public void SetImage(string image) => Image = image;
}