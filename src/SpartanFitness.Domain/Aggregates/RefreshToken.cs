using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public class RefreshToken : AggregateRoot<RefreshTokenId, Guid>
{
  public string JwtId { get; private set; }
  public DateTime CreationDateTime { get; private set; }
  public DateTime ExpiryDateTime { get; private set; }
  public bool Used { get; private set; }
  public bool Invalidated { get; private set; }
  public UserId UserId { get; private set; }

  private RefreshToken(
    RefreshTokenId id,
    string jwtId,
    DateTime creationDateTime,
    DateTime expiryDateTime,
    bool used,
    bool invalidated,
    UserId userId)
    : base(id)
  {
    JwtId = jwtId;
    CreationDateTime = creationDateTime;
    ExpiryDateTime = expiryDateTime;
    Used = used;
    Invalidated = invalidated;
    UserId = userId;
  }

#pragma warning disable CS8618
  private RefreshToken()
  {
  }
#pragma warning restore CS8618

  public static RefreshToken Create(
    string jwtId,
    DateTime expiryDateTime,
    UserId userId)
  {
    return new RefreshToken(
      RefreshTokenId.CreateUnique(),
      jwtId,
      DateTime.UtcNow,
      expiryDateTime,
      false,
      false,
      userId);
  }

  public void Use() => Used = true;
  public void Invalidate() => Invalidated = true;
}