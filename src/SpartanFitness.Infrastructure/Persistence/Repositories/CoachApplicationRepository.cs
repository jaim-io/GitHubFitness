using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class CoachApplicationRepository : ICoachApplicationRepository
{
  private readonly SpartanFitnessDbContext _dbContext;

  public CoachApplicationRepository(SpartanFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(CoachApplication coachApplication)
  {
    _dbContext.Add(coachApplication);

    await _dbContext.SaveChangesAsync();
  }

  public async Task<bool> AreRelatedAsync(CoachApplicationId id, UserId userId)
  {
    return await _dbContext.CoachApplications.AnyAsync(ca => ca.Id == id && ca.UserId == userId);
  }

  public async Task<bool> ExistsAsync(CoachApplicationId id)
  {
    return await _dbContext.CoachApplications.AnyAsync(ca => ca.Id == id);
  }

  public async Task<CoachApplication?> GetByIdAsync(CoachApplicationId id)
  {
    return await _dbContext.CoachApplications.FirstOrDefaultAsync(ca => ca.Id == id);
  }

  public async Task<bool> IsOpenAsync(CoachApplicationId id)
  {
    return await _dbContext.CoachApplications.AnyAsync(ca => ca.Id == id && ca.Status == Status.Pending);
  }

  public async Task<bool> HasPendingApplications(UserId id)
  {
    return await _dbContext.CoachApplications.AnyAsync(ca => ca.UserId == id && ca.Status == Status.Pending);
  }

  public async Task UpdateAsync(CoachApplication coachApplication)
  {
    _dbContext.Update(coachApplication);
    await _dbContext.SaveChangesAsync();
  }
}