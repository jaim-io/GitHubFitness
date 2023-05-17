using Microsoft.AspNetCore.Http;

using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IImageRepository
{
  public Task SaveAsync<T>(AggregateRootId<Guid> id, IFormFile image)
    where T : AggregateRootId<Guid>;

  public Task<(byte[] FileContents, string ContentType)> GetAsync<T>(AggregateRootId<Guid> id)
    where T : AggregateRootId<Guid>;

  public void Delete<T>(AggregateRootId<Guid> id)
    where T : AggregateRootId<Guid>;

  public bool Exists<T>(AggregateRootId<Guid> id)
    where T : AggregateRootId<Guid>;
}