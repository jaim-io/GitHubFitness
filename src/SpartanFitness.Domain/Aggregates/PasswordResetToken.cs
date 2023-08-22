using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public class PasswordResetToken : AggregateRoot<PasswordResetTokenId, Guid>
{
  public string Value { get; private set; }
  public DateTime CreationDateTime { get; private set; }
  public DateTime ExpiryDateTime { get; private set; }
  public bool Used { get; private set; }
  public bool Invalidated { get; private set; }
  public UserId UserId { get; private set; }

  private PasswordResetToken(
    PasswordResetTokenId id,
    string value,
    DateTime creationDateTime,
    DateTime expiryDateTime,
    bool used,
    bool invalidated,
    UserId userId)
    : base(id)
  {
    Value = value;
    CreationDateTime = creationDateTime;
    ExpiryDateTime = expiryDateTime;
    Used = used;
    Invalidated = invalidated;
    UserId = userId;
  }

#pragma warning disable CS8618
  private PasswordResetToken()
  {
  }
#pragma warning restore CS8618

  public static PasswordResetToken Create(
    string value,
    DateTime expires,
    UserId userId)
  {
    return new PasswordResetToken(
      PasswordResetTokenId.CreateUnique(),
      value,
      DateTime.Now,
      expires,
      false,
      false,
      userId);
  }

  public void Use() => Used = true;
  public void Invalidate() => Invalidated = true;
}