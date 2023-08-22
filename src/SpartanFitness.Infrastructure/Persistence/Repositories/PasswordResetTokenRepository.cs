using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
  private readonly SpartanFitnessDbContext _dbContext;

  public PasswordResetTokenRepository(SpartanFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(PasswordResetToken token)
  {
    _dbContext.Add(token);
    await _dbContext.SaveChangesAsync();
  }

  public async Task<List<PasswordResetToken>> GetByUserIdAsync(UserId id)
  {
    return await _dbContext.PasswordResetTokens
      .Where(pwrt => pwrt.UserId == id)
      .ToListAsync();
  }

  public async Task UpdateAsync(PasswordResetToken token)
  {
    _dbContext.Update(token);
    await _dbContext.SaveChangesAsync();
  }

  public async Task InvalidateAllAsync()
  {
    await _dbContext.PasswordResetTokens
      .Where(pwrt => !pwrt.Used && !pwrt.Invalidated)
      .ExecuteUpdateAsync(setter => setter.SetProperty(
        pwrt => pwrt.Invalidated,
        true));

    await _dbContext.SaveChangesAsync();
  }
}