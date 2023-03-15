using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IMuscleGroupRepository {
    Task AddAsync(MuscleGroup muscleGroup);
    Task<bool> ExistsAsync(MuscleGroupId id);
}