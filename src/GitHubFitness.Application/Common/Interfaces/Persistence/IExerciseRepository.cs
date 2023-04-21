using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Application.Common.Interfaces.Persistence;

public interface IExerciseRepository
{
  Task AddAsync(Exercise exercise);
  Task<List<Exercise>> GetAll();
  Task<Exercise?> GetByIdAsync(ExerciseId id);
  Task<bool> ExistsAsync(IEnumerable<ExerciseId> ids);
  Task<IEnumerable<MuscleGroupId>> GetMuscleGroupIds(IEnumerable<ExerciseId> exerciseIds);
}