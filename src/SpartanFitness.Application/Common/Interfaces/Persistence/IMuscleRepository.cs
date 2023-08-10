using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IMuscleRepository
{
  Task AddAsync(Muscle muscle, MuscleGroupId muscleGroupId);
  Task<IEnumerable<Muscle>> GetAllAsync();
  Task<List<Muscle>> GetBySearchQueryAsync(string searchQuery);
  Task<List<Muscle>> GetBySearchQueryAndIdsAsync(string searchQuery, List<MuscleId> ids);
  Task<Muscle?> GetByIdAsync(MuscleId id);
  Task<List<Muscle>> GetByIdAsync(List<MuscleId> ids);
  Task<Muscle?> GetByNameAsync(string name);
  Task<bool> ExistsAsync(MuscleId id);
  Task<bool> ExistsAsync(IEnumerable<MuscleId> ids);
  Task UpdateAsync(Muscle muscle); 
}