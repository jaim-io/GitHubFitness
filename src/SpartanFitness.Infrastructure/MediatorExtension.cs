using MediatR;

using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Infrastructure.Persistence;

namespace SpartanFitness.Infrastructure;

public static class MediatorExtension
{
  public static async Task DispatchDomainEventsAsync<TId>(this IMediator mediator, SpartanFitnessDbContext ctx)
    where TId : ValueObject
  {
    var domainEntities = ctx.ChangeTracker
      .Entries<Entity<TId>>()
      .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any());

    var domainEvents = domainEntities
      .SelectMany(e => e.Entity.DomainEvents)
      .ToList();

    domainEntities.ToList()
      .ForEach(entity => entity.Entity.ClearDomainEvents());

    foreach (var domainEvent in domainEvents)
    {
      await mediator.Publish(domainEvent);
    }
  }
}