using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class WorkoutRepository
  : IWorkoutRepository
{
  private readonly SpartanFitnessDbContext _dbContext;

  public WorkoutRepository(SpartanFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(Workout workout)
  {
    _dbContext.Add(workout);

    await _dbContext.SaveEntitiesAsync<WorkoutId>();
  }

  public Task<Workout?> GetByIdAsync(WorkoutId id)
  {
    return _dbContext.Workouts.FirstOrDefaultAsync(w => w.Id == id);
  }
}