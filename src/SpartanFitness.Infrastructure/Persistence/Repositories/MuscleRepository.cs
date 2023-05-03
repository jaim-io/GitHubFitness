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

  public async Task<Muscle?> GetByIdAsync(MuscleId id)
  {
    return await _dbContext.Muscles.FirstOrDefaultAsync(m => m.Id == id);
  }

  public async Task<Muscle?> GetByNameAsync(string name)
  {
    return await _dbContext.Muscles.FirstOrDefaultAsync(m => m.Name == name);
  }
}