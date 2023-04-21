using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IWorkoutRepository
{
  Task AddAsync(Workout workout);
  Task<Workout?> GetByIdAsync(WorkoutId id);
}