using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IWorkoutRepository
{
  Task AddAsync(Workout workout);
}