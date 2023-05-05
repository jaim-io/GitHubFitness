using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public sealed class UserId : AggregateRootId<Guid>
{
  public override Guid Value { get; protected set; }

  public UserId(Guid value)
  {
    Value = value;
  }

  public static UserId Create(Guid value)
  {
    return new(value);
  }

  public static UserId Create(string value)
  {
    return new(Guid.Parse(value));
  }

  public static UserId CreateUnique()
  {
    return new(Guid.NewGuid());
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}