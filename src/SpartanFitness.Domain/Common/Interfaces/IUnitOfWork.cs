using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
  Task<bool> SaveEntitiesAsync<TId>(CancellationToken cancellationToken = default(CancellationToken)) 
    where TId : ValueObject;
}