namespace SpartanFitness.Domain.Common.Models;

public interface IUnitOfWork : IDisposable
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
  Task<bool> SaveEntitiesAsync<TId>(CancellationToken cancellationToken = default(CancellationToken)) 
    where TId : ValueObject;
}