using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public class Muscle : AggregateRoot<MuscleId, Guid>
{
  public string Name { get; set; }
  public string Description { get; set; }
  public string Image { get; set; }
  public DateTime CreatedDateTime { get; set; }
  public DateTime UpdatedDateTime { get; set; }

  public Muscle(
    MuscleId id,
    string name,
    string description,
    string image,
    DateTime createdDateTime,
    DateTime updatedDateTime)
    : base(id)
  {
    Name = name;
    Description = description;
    Image = image;
    CreatedDateTime = createdDateTime;
    UpdatedDateTime = updatedDateTime;
  }

#pragma warning disable CS8618
  private Muscle()
  {
  }
#pragma warning restore CS8618

  public static Muscle Create(
    string name,
    string description,
    string image)
  {
    return new(
      MuscleId.CreateUnique(),
      name,
      description,
      image,
      DateTime.UtcNow,
      DateTime.UtcNow);
  }
}