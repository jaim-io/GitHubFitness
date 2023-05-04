using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class MuscleGroupRepository : IMuscleGroupRepository
{
  private readonly SpartanFitnessDbContext _dbContext;

  public MuscleGroupRepository(SpartanFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(MuscleGroup muscleGroup)
  {
    _dbContext.Add(muscleGroup);

    await _dbContext.SaveEntitiesAsync<MuscleGroupId>();
  }

  public IEnumerable<MuscleGroup> GetAllWithFilter(Func<MuscleGroup, bool> filter)
  {
    return _dbContext.MuscleGroups.Where(filter).ToList();
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

    return !results.Contains(false);
  }

  public async Task<MuscleGroup?> GetByIdAsync(MuscleGroupId id)
  {
    return await _dbContext.MuscleGroups.FirstOrDefaultAsync(mg => mg.Id == id);
  }

  public async Task<MuscleGroup?> GetByNameAsync(string name)
  {
    return await _dbContext.MuscleGroups.FirstOrDefaultAsync(mg => mg.Name == name);
  }

  public async Task<IEnumerable<MuscleGroup>> GetAllAsync()
  {
    return await _dbContext.MuscleGroups.ToListAsync();
  }
}