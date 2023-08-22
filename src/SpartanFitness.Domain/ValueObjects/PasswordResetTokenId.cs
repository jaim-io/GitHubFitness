using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public class PasswordResetTokenId : AggregateRootId<Guid>
{
  public override Guid Value { get; protected set; }

  public PasswordResetTokenId(Guid value)
  {
    Value = value;
  }

  public static PasswordResetTokenId Create(Guid value)
  {
    return new(value);
  }

  public static PasswordResetTokenId Create(string value)
  {
    return new(Guid.Parse(value));
  }

  public static PasswordResetTokenId CreateUnique()
  {
    return new(Guid.NewGuid());
  }

  public override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}