using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.Events;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public enum Status
{
  Pending,
  Approved,
  Denied,
}

public class CoachApplication : AggregateRoot<CoachApplicationId>
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

  public void SetClosedDateTime(DateTime dateTime)
  {
    ClosedDateTime = dateTime > CreatedDateTime
      ? dateTime
      : throw new ArgumentException($"{nameof(dateTime)} should occur after {nameof(CreatedDateTime)}");
  }

  public void Approve(string remarks)
  {
    Status = Status.Approved;

    SetRemarks(remarks);
    SetClosedDateTime(DateTime.UtcNow);

    AddDomainEvent(new CoachApplicationApproved(Id, UserId));
  }

  public void Deny(string remarks)
  {
    Status = Status.Denied;
    
    SetRemarks(remarks);
    SetClosedDateTime(DateTime.UtcNow);
  }
}