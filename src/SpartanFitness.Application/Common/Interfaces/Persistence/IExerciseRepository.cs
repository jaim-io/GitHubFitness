using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IExerciseRepository
{
  Task AddAsync(Exercise exercise);
  IEnumerable<Exercise> GetAll(Func<Exercise, bool>? filter = null);
  Task<Exercise?> GetByIdAsync(ExerciseId id);
  Task<bool> ExistsAsync(IEnumerable<ExerciseId> ids);
  Task<IEnumerable<MuscleGroupId>> GetMuscleGroupIds(IEnumerable<ExerciseId> exerciseIds);
}