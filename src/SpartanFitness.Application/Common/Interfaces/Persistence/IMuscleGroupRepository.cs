using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IMuscleGroupRepository {
    Task AddAsync(MuscleGroup muscleGroup);
}