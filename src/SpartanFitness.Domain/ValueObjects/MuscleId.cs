using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public class MuscleId : ValueObject
{
  public Guid Value { get; private set; }

  public MuscleId(Guid value)
  {
    Value = value;
  }

  public static MuscleId Create(Guid value)
  {
    return new(value);
  }

  public static MuscleId Create(string value)
  {
    return new(Guid.Parse(value));
  }

  public static MuscleId CreateUnique()
  {
    return new(Guid.NewGuid());
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}