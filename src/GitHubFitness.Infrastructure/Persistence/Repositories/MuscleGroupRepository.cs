using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace GitHubFitness.Infrastructure.Persistence.Repositories;

public class MuscleGroupRepository : IMuscleGroupRepository
{
  private readonly GitHubFitnessDbContext _dbContext;

  public MuscleGroupRepository(GitHubFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(MuscleGroup muscleGroup)
  {
    _dbContext.Add(muscleGroup);

    await _dbContext.SaveEntitiesAsync<MuscleGroupId>();
  }

  public async Task<bool> ExistsAsync(MuscleGroupId id)
  {
    return await _dbContext.MuscleGroups.AnyAsync(mg => mg.Id == id);
  }

  public async Task<bool> ExistsAsync(IEnumerable<MuscleGroupId> ids)
  {
    var results = new List<bool>();
    foreach (var id in ids)
    {
      results.Add(await _dbContext.MuscleGroups.AnyAsync(mg => mg.Id == id));
    }

    return results.Contains(false)
      ? false
      : true;
  }

  public async Task<MuscleGroup?> GetByIdAsync(MuscleGroupId id)
  {
    return await _dbContext.MuscleGroups.FirstOrDefaultAsync(mg => mg.Id == id);
  }
}