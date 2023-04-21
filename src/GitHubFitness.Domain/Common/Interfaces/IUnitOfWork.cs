using GitHubFitness.Domain.Common.Models;

namespace GitHubFitness.Domain.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
  Task<bool> SaveEntitiesAsync<TId>(CancellationToken cancellationToken = default(CancellationToken)) 
    where TId : ValueObject;
}