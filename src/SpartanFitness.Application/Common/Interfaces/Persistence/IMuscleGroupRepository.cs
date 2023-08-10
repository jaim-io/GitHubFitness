using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IMuscleGroupRepository
{
  Task AddAsync(MuscleGroup muscleGroup);
  Task<MuscleGroup?> GetByIdAsync(MuscleGroupId id);
  Task<List<MuscleGroup>> GetByIdAsync(List<MuscleGroupId> ids);
  Task<MuscleGroup?> GetByNameAsync(string name);
  Task<IEnumerable<MuscleGroup>> GetAllAsync();
  Task<List<MuscleGroup>> GetBySearchQueryAsync(string searchQuery);
  Task<List<MuscleGroup>> GetBySearchQueryAndIdsAsync(string searchQuery, List<MuscleGroupId> ids);
  Task<List<MuscleGroup>> GetByMuscleIdAsync(MuscleId id);
  Task<List<MuscleGroup>> GetByMuscleIdAsync(List<MuscleId> ids);
  Task<bool> ExistsAsync(MuscleGroupId id);
  Task<bool> ExistsAsync(IEnumerable<MuscleGroupId> ids);
  Task UpdateAsync(MuscleGroup muscleGroup);
}