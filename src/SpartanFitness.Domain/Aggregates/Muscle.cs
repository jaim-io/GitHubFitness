using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public class Muscle : Entity<MuscleId>
{
  public string Name { get; set; }
  public string Description { get; set; }
  public MuscleGroupId MuscleGroupId { get; set; }
  public string Image { get; set; }
  public DateTime CreatedDateTime { get; set; }
  public DateTime UpdatedDateTime { get; set; }

  public Muscle(
    MuscleId id,
    string name,
    string description,
    string image,
    DateTime createdDateTime,
    DateTime updatedDateTime,
    MuscleGroupId muscleGroupId)
    : base(id)
  {
    Name = name;
    Description = description;
    Image = image;
    CreatedDateTime = createdDateTime;
    UpdatedDateTime = updatedDateTime;
    MuscleGroupId = muscleGroupId;
  }

#pragma warning disable CS8618
  private Muscle()
  {
  }
#pragma warning restore CS8618

  public static Muscle Create(
    string name,
    string description,
    string image,
    MuscleGroupId muscleGroupId)
  {
    return new(
      MuscleId.CreateUnique(),
      name,
      description,
      image,
      DateTime.UtcNow,
      DateTime.UtcNow,
      muscleGroupId);
  }
}