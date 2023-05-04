using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IMuscleRepository
{
  Task AddAsync(Muscle muscle);
  Task<IEnumerable<Muscle>> GetAllAsync();
  Task<List<Muscle>> GetBySearchQueryAsync(string searchQuery);
  Task<List<Muscle>> GetBySearchQueryAsync(string searchQuery, MuscleGroupId id);
  Task<List<Muscle>> GetByMuscleGroupIdAsync(MuscleGroupId id);
  Task<Muscle?> GetByIdAsync(MuscleId id);
  Task<Muscle?> GetByNameAsync(string name);
}