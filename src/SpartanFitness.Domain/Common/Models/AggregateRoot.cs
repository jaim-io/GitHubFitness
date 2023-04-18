using MediatR;

namespace SpartanFitness.Domain.Common.Models;

public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
  private List<IDomainEvent> _domainEvents = new();
  public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

  protected AggregateRoot(TId id)
      : base(id)
  {
  }

  public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
  public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
  public void ClearDomainEvents() => _domainEvents = new();

#pragma warning disable CS8618
  protected AggregateRoot()
  {
  }
#pragma warning restore CS8618
}