using Microsoft.EntityFrameworkCore;

using Quartz;

using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Infrastructure.Persistence;

namespace SpartanFitness.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class SpartanFitnessDbCleanupBackgroundJob : IJob
{
  private readonly SpartanFitnessDbContext _dbContext;
  private readonly IDateTimeProvider _dateTimeProvider;

  public SpartanFitnessDbCleanupBackgroundJob(
    SpartanFitnessDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
  {
    _dbContext = dbContext;
    _dateTimeProvider = dateTimeProvider;
  }

  public async Task Execute(IJobExecutionContext context)
  {
    // Delete expired - valid - unused PasswordResetTokens
    await _dbContext.PasswordResetTokens
      .Where(pwrt => !pwrt.Invalidated && !pwrt.Used && pwrt.ExpiryDateTime > _dateTimeProvider.UtcNow)
      .ExecuteDeleteAsync();
    
    // Delete expired - valid - unused RefreshTokens
    await _dbContext.RefreshTokens
      .Where(rt => !rt.Invalidated && !rt.Used && rt.ExpiryDateTime > _dateTimeProvider.UtcNow)
      .ExecuteDeleteAsync();
    
    // Delete leftover Coach entities
    await _dbContext.Database.ExecuteSqlAsync($@"
        DELETE c
      	FROM Coaches as c
        LEFT JOIN Users as u
            ON u.Id = c.UserId
        WHERE u.Id IS NULL");
        
    // Delete leftover Administrator entities
    await _dbContext.Database.ExecuteSqlAsync($@"
        DELETE a
      	FROM Administrators as a
        LEFT JOIN Users as u
            ON u.Id = a.UserId
        WHERE u.Id IS NULL");

    await _dbContext.SaveChangesAsync();
  }
}