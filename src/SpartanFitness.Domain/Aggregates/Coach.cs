using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class Coach : AggregateRoot<CoachId, Guid>
{
  public UserId UserId { get; private set; }
  public string Biography { get; private set; }
  public SocialMedia SocialMedia { get; private set; }
  public DateTime CreatedDateTime { get; private set; }
  public DateTime UpdatedDateTime { get; private set; }

  private Coach(
    CoachId id,
    UserId userId,
    string biography,
    SocialMedia socialMedia,
    DateTime createdDateTime,
    DateTime updatedDateTime)
    : base(id)
  {
    UserId = userId;
    Biography = biography;
    SocialMedia = socialMedia;
    CreatedDateTime = createdDateTime;
    UpdatedDateTime = updatedDateTime;
  }

#pragma warning disable CS8618
  private Coach()
  {
  }
#pragma warning restore CS8618

  public static Coach Create(
    UserId userId,
    string biography,
    SocialMedia socialMedia)
  {
    return new(
      CoachId.CreateUnique(),
      userId,
      biography,
      socialMedia,
      DateTime.UtcNow,
      DateTime.UtcNow);
  }
}