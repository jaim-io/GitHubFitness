using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IWorkoutRepository
{
  Task AddAsync(Workout workout);
  Task<Workout?> GetByIdAsync(WorkoutId id);
  Task<List<Workout>> GetByIdAsync(List<WorkoutId> ids);
  Task<List<Workout>> GetByExerciseId(ExerciseId id);
  Task<bool> ExistsAsync(WorkoutId id);
  Task<bool> ExistsAsync(List<WorkoutId> ids);
  Task<IEnumerable<Workout>> GetAllAsync();
  Task<List<Workout>> GetBySearchQueryAsync(string searchQuery);
  Task<List<Workout>> GetBySearchQueryAndIdsAsync(string searchQuery, List<WorkoutId> ids);
  Task UpdateAsync(Workout workout);
  Task RemoveAsync(Workout workout);
  Task<List<User>> GetSubscribers(WorkoutId id);
  Task<List<User>> GetSubscribers(List<WorkoutId> ids);
}