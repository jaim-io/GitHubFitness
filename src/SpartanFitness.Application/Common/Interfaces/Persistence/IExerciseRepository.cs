using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IExerciseRepository
{
  Task AddAsync(Exercise exercise);
  Task<IEnumerable<Exercise>> GetAllAsync();
  Task<List<Exercise>> GetBySearchQueryAsync(string searchQuery);
  Task<Exercise?> GetByIdAsync(ExerciseId id);
  Task<Exercise?> GetByNameAsync(string name);
  Task<bool> ExistsAsync(IEnumerable<ExerciseId> ids);
  Task<IEnumerable<MuscleGroupId>> GetMuscleGroupIds(IEnumerable<ExerciseId> exerciseIds);
}