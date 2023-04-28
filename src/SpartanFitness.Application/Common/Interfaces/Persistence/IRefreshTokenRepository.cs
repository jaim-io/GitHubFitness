using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IRefreshTokenRepository
{
  Task<RefreshToken?> GetByIdAsync(RefreshTokenId id);
  Task AddAsync(RefreshToken token);
  Task UpdateAsync(RefreshToken token);
}