using Microsoft.Data.SqlClient;
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

  public async Task AddAsync(Muscle muscle, MuscleGroupId muscleGroupId)
  {
    _dbContext.Add(muscle);
    await _dbContext.SaveChangesAsync();

    // Adds the muscle to the muscleGroup
    await _dbContext.Database.ExecuteSqlAsync($@"
      INSERT INTO MuscleGroupMuscleIds (MuscleId, MuscleGroupId)
      VALUES ({muscle.Id.Value}, {muscleGroupId.Value})");
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

  public async Task<List<Muscle>> GetBySearchQueryAndIdsAsync(string searchQuery, List<MuscleId> ids)
  {
    if (!ids.Any())
    {
      return new();
    }

    var searchQueryParam = $"@p{0}";
    var idParameters = string.Join(", ", ids.Select((_, i) => $"@p{i + 1}"));
    var query = $@"
      SELECT *
      FROM Muscles 
      WHERE Id in ({idParameters}) 
        and (LOWER(Name) LIKE '%' + {searchQueryParam} + '%' 
          or LOWER(Description) LIKE '%' + {searchQueryParam} + '%')
    ";

    var sqlSearchParameter = new SqlParameter($"@p{0}", searchQuery.ToLower());
    var sqlParameters = ids
      .Select((id, i) => new SqlParameter($"@p{i + 1}", id.Value))
      .ToList();
    sqlParameters.Add(sqlSearchParameter);

    return await _dbContext.Muscles
      .FromSqlRaw(query, sqlParameters.ToArray())
      .ToListAsync();
  }

  public async Task<Muscle?> GetByIdAsync(MuscleId id)
  {
    return await _dbContext.Muscles.FirstOrDefaultAsync(m => m.Id == id);
  }

  public async Task<List<Muscle>> GetByIdAsync(List<MuscleId> ids)
  {
    return await _dbContext.Muscles
      .Where(m => ids.Contains(m.Id))
      .ToListAsync();
  }

  public async Task<Muscle?> GetByNameAsync(string name)
  {
    return await _dbContext.Muscles.FirstOrDefaultAsync(m => m.Name == name);
  }

  public async Task<bool> ExistsAsync(MuscleId id)
  {
    return await _dbContext.Muscles.AnyAsync(m => m.Id == id);
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

  public async Task UpdateAsync(Muscle muscle)
  {
    _dbContext.Update(muscle);
    await _dbContext.SaveChangesAsync();
  }
}