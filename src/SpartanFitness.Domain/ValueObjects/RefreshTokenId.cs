using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public class RefreshTokenId : ValueObject
{
  public Guid Value { get; private set; }

  public RefreshTokenId(Guid value)
  {
    Value = value;
  }

  public static RefreshTokenId Create(Guid value)
  {
    return new(value);
  }

  public static RefreshTokenId Create(string value)
  {
    return new(Guid.Parse(value));
  }

  public static RefreshTokenId CreateUnique()
  {
    return new(Guid.NewGuid());
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}