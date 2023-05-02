using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IMuscleGroupRepository
{
  Task AddAsync(MuscleGroup muscleGroup);
  Task<MuscleGroup?> GetByIdAsync(MuscleGroupId id);
  Task<IEnumerable<MuscleGroup>> GetAllAsync();
  IEnumerable<MuscleGroup> GetAllWithFilter(Func<MuscleGroup, bool> filter);
  Task<bool> ExistsAsync(MuscleGroupId id);
  Task<bool> ExistsAsync(IEnumerable<MuscleGroupId> ids);
}