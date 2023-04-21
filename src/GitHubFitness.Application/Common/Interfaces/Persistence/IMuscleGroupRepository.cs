using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Application.Common.Interfaces.Persistence;

public interface IMuscleGroupRepository {
    Task AddAsync(MuscleGroup muscleGroup);
    Task<MuscleGroup?> GetByIdAsync(MuscleGroupId id);
    Task<bool> ExistsAsync(MuscleGroupId id);
    Task<bool> ExistsAsync(IEnumerable<MuscleGroupId> ids);
}