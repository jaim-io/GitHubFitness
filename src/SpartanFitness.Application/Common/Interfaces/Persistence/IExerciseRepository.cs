using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IExerciseRepository
{
  Task AddAsync(Exercise exercise);
  IEnumerable<Exercise> GetAllWithFilter(Func<Exercise, bool> filter);
  Task<IEnumerable<Exercise>> GetAllAsync();
  Task<Exercise?> GetByIdAsync(ExerciseId id);
  Task<Exercise?> GetByNameAsync(string name);
  Task<bool> ExistsAsync(IEnumerable<ExerciseId> ids);
  Task<IEnumerable<MuscleGroupId>> GetMuscleGroupIds(IEnumerable<ExerciseId> exerciseIds);
}