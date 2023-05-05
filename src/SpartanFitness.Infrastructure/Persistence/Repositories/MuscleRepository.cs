using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class MuscleRepository : IMuscleRepository
{
  private readonly SpartanFitnessDbContext _dbContext;

  public MuscleRepository(SpartanFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(Muscle muscle)
  {
    _dbContext.Add(muscle);
    await _dbContext.SaveEntitiesAsync<MuscleId>();
  }

  public async Task<IEnumerable<Muscle>> GetAllAsync()
  {
    return await _dbContext.Muscles.ToListAsync();
  }

  public IEnumerable<Muscle> GetAllWithFilter(Func<Muscle, bool> filter)
  {
    return _dbContext.Muscles.Where(filter);
  }

  public async Task<List<Muscle>> GetBySearchQueryAsync(string searchQuery)
  {
    string query = searchQuery.ToLower();

    return await _dbContext.Muscles
      .Where(m => (m.Name.ToLower().Contains(query) || m.Description.ToLower().Contains(query)))
      .ToListAsync();
  }

  public async Task<List<Muscle>> GetBySearchQueryAsync(string searchQuery, MuscleGroupId id)
  {
    string query = searchQuery.ToLower();

    return await _dbContext.Muscles
      .Where(m => (m.Name.ToLower().Contains(query) || m.Description.ToLower().Contains(query)) &&
                  m.MuscleGroupId == id)
      .ToListAsync();
  }

  public async Task<List<Muscle>> GetByMuscleGroupIdAsync(MuscleGroupId id)
  {
    return await _dbContext.Muscles.Where(m => m.MuscleGroupId == id).ToListAsync();
  }

  public async Task<Muscle?> GetByIdAsync(MuscleId id)
  {
    return await _dbContext.Muscles.FirstOrDefaultAsync(m => m.Id == id);
  }

  public async Task<Muscle?> GetByNameAsync(string name)
  {
    return await _dbContext.Muscles.FirstOrDefaultAsync(m => m.Name == name);
  }

  public async Task<bool> ExistsAsync(IEnumerable<MuscleId> ids)
  {
    var result = new List<bool>();
    foreach (var id in ids)
    {
      result.Add(await _dbContext.Muscles.AnyAsync(m => m.Id == id));
    }

    return !result.Contains(false);
  }
}