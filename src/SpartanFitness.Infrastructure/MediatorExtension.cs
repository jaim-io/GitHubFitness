using MediatR;

using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Infrastructure.Persistence;

namespace SpartanFitness.Infrastructure;

public static class MediatorExtension
{
  public static async Task DispatchDomainEventsAsync<TId>(this IMediator mediator, SpartanFitnessDbContext ctx)
    where TId : ValueObject
  {
    var domainAggregates = ctx.ChangeTracker
      .Entries<AggregateRoot<TId>>()
      .Where(a => a.Entity.DomainEvents != null && a.Entity.DomainEvents.Any());

    var domainEvents = domainAggregates
      .SelectMany(da => da.Entity.DomainEvents)
      .ToList();

    domainAggregates.ToList()
      .ForEach(aggregate => aggregate.Entity.ClearDomainEvents());

    foreach (var domainEvent in domainEvents)
    {
      await mediator.Publish(domainEvent);
    }
  }
}