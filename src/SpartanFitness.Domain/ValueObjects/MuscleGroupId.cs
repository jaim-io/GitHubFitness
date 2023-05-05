using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public sealed class MuscleGroupId : AggregateRootId<Guid>
{
  public override Guid Value { get; protected set; }

  public MuscleGroupId(Guid value)
  {
    Value = value;
  }

  public static MuscleGroupId Create(Guid value)
  {
    return new(value);
  }

  public static MuscleGroupId Create(string value)
  {
    return new(Guid.Parse(value));
  }

  public static MuscleGroupId CreateUnique()
  {
    return new(Guid.NewGuid());
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}