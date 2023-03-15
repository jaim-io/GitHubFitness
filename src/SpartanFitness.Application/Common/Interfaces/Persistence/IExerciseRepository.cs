using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IExerciseRepository {
    Task AddAsync(Exercise exercise);
}