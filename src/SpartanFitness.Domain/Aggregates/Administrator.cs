using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class Administrator : AggregateRoot<AdministratorId, Guid>
{
    public UserId UserId { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    private Administrator(
        AdministratorId id,
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
    private Administrator()
    {
    }
#pragma warning restore CS8618

    public static Administrator Create(
        UserId userId)
    {
        return new(
            AdministratorId.CreateUnique(),
            userId,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}