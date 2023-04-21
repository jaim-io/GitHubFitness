using GitHubFitness.Domain.Common.Models;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Domain.Aggregates;

public sealed class Coach : AggregateRoot<CoachId>
{
    public UserId UserId { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    private Coach(
        CoachId id,
        UserId userId,
        DateTime createdDateTime,
        DateTime updatedDateTime)
        : base(id)
    {
        UserId = userId;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

#pragma warning disable CS8618
    private Coach()
    {
    }
#pragma warning restore CS8618

    public static Coach Create(
        UserId userId)
    {
        return new(
            CoachId.CreateUnique(),
            userId,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}