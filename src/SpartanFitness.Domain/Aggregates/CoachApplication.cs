using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public enum Status
{
    Pending,
    Approved,
    Denied,
}

public class CoachApplication : Entity<CoachApplicationId>
{
    public string Motivation { get; private set; }
    public string Remarks { get; private set; }
    public Status Status { get; private set; }
    public UserId UserId { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime ClosedDateTime { get; private set; }

    // Coach's Fields

    private CoachApplication(
        CoachApplicationId id,
        string motivation,
        string remarks,
        Status status,
        UserId userId,
        DateTime createdDateTime,
        DateTime closedDateTime)
        : base(id)
    {
        Motivation = motivation;
        Remarks = remarks;
        Status = status;
        UserId = userId;
        CreatedDateTime = createdDateTime;
        ClosedDateTime = closedDateTime;
    }

#pragma warning disable CS8618
    private CoachApplication()
    {
    }
#pragma warning restore CS8618

    public static CoachApplication Create(
        string motivation,
        Status status,
        UserId userId,
        string remarks = "")
    {
        return new(
            CoachApplicationId.CreateUnique(),
            motivation,
            remarks,
            status,
            userId,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }

    public void SetRemarks(string remarks)
    {
        Remarks = remarks ?? throw new ArgumentNullException(nameof(remarks));
    }

    public void SetStatus(Status status, DateTime closedDateTime = default)
    {
        if (status == Status.Pending)
        {
            Status = closedDateTime == default
                ? status
                : throw new ArgumentException($"{nameof(closedDateTime)} should be default when the value of {nameof(status)} is equal to Status.Pending");
        }
        else
        {
            Status = closedDateTime != default
                ? status
                : throw new ArgumentException($"{nameof(closedDateTime)} should not be default when the value of {nameof(status)} is equal to Status.Approved or Status.Denied");

            ClosedDateTime = closedDateTime > CreatedDateTime
                ? closedDateTime
                : throw new ArgumentException($"{nameof(closedDateTime)} should occur after {nameof(CreatedDateTime)}");
        }
    }
}