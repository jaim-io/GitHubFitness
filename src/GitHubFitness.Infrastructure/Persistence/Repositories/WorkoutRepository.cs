using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace GitHubFitness.Infrastructure.Persistence.Repositories;

public class WorkoutRepository
  : IWorkoutRepository
{
  private readonly GitHubFitnessDbContext _dbContext;

  public WorkoutRepository(GitHubFitnessDbContext dbContext)
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