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
}