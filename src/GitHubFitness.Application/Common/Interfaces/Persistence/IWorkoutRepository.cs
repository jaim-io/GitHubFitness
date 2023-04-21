using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Application.Common.Interfaces.Persistence;

public interface IWorkoutRepository
{
  Task AddAsync(Workout workout);
  Task<Workout?> GetByIdAsync(WorkoutId id);
}