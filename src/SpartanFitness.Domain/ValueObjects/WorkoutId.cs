using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public sealed class WorkoutId : AggregateRootId<Guid>
{
  public override Guid Value { get; protected set; }

  public WorkoutId(Guid value)
  {
    Value = value;
  }

  public static WorkoutId Create(Guid value)
  {
    return new(value);
  }

  public static WorkoutId Create(string value)
  {
    return new(Guid.Parse(value));
  }

  public static WorkoutId CreateUnique()
  {
    return new(Guid.NewGuid());
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}