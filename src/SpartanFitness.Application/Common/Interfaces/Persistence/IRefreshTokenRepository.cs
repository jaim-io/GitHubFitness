using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IRefreshTokenRepository
{
  Task<RefreshToken?> GetByIdAsync(RefreshTokenId id);
  Task<List<RefreshToken>> GetByUserIdAsync(UserId userId);
  Task AddAsync(RefreshToken token);
  Task UpdateAsync(RefreshToken token);
  Task UpdateRangeAsync(List<RefreshToken> tokens);
  Task InvalidateAllAsync();
}