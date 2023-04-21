using GitHubFitness.Domain.Common.Models;
using GitHubFitness.Infrastructure.Persistence;

using MediatR;

namespace GitHubFitness.Infrastructure;

public static class MediatorExtension
{
  public static async Task DispatchDomainEventsAsync<TId>(this IMediator mediator, GitHubFitnessDbContext ctx)
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