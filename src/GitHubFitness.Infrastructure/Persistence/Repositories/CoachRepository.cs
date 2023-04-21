using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace GitHubFitness.Infrastructure.Persistence.Repositories;

public class CoachRepository : ICoachRepository
{
  private readonly GitHubFitnessDbContext _dbContext;

  public CoachRepository(GitHubFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(Coach coach)
  {
    _dbContext.Add(coach);

    await _dbContext.SaveEntitiesAsync<CoachId>();
  }

  public async Task<Coach?> GetByIdAsync(CoachId id)
  {
    return await _dbContext.Coaches.FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<Coach?> GetByUserIdAsync(UserId id)
  {
    return await _dbContext.Coaches.FirstOrDefaultAsync(c => c.UserId == id);
  }
}