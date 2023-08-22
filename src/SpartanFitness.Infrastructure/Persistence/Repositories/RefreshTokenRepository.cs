using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository
  : IRefreshTokenRepository
{
  private readonly SpartanFitnessDbContext _dbContext;

  public RefreshTokenRepository(SpartanFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<List<RefreshToken>> GetByUserIdAsync(UserId userId)
  {
    return await _dbContext.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();
  }

  public async Task AddAsync(RefreshToken token)
  {
    _dbContext.Add(token);

    await _dbContext.SaveChangesAsync();
  }

  public async Task<RefreshToken?> GetByIdAsync(RefreshTokenId id)
  {
    return await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == id);
  }

  public async Task UpdateAsync(RefreshToken token)
  {
    _dbContext.Update(token);
    await _dbContext.SaveChangesAsync();
  }

  public async Task UpdateRangeAsync(List<RefreshToken> tokens)
  {
    _dbContext.UpdateRange(tokens);
    await _dbContext.SaveChangesAsync();
  }

  public async Task InvalidateAllAsync()
  {
    await _dbContext.RefreshTokens
      .Where(rt => !rt.Used && !rt.Invalidated)
      .ExecuteUpdateAsync(setter => setter.SetProperty(
        rt => rt.Invalidated,
        true));

    await _dbContext.SaveChangesAsync();
  }
}