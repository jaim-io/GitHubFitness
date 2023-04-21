using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace GitHubFitness.Infrastructure.Persistence.Repositories;

public class CoachApplicationRepository : ICoachApplicationRepository
{
  private readonly GitHubFitnessDbContext _dbContext;

  public CoachApplicationRepository(GitHubFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(CoachApplication coachApplication)
  {
    _dbContext.Add(coachApplication);

    await _dbContext.SaveEntitiesAsync<CoachApplicationId>();
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

  public async Task UpdateAsync(CoachApplication coachApplication)
  {
    _dbContext.Update(coachApplication);
    await _dbContext.SaveEntitiesAsync<CoachApplicationId>();
  }
}