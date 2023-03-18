using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;

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

    await _dbContext.SaveChangesAsync();
  }
}