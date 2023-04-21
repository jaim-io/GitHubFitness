using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace GitHubFitness.Infrastructure.Persistence.Repositories;

public class ExerciseRepository
  : IExerciseRepository
{
  private readonly GitHubFitnessDbContext _dbContext;

  public ExerciseRepository(GitHubFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(Exercise exercise)
  {
    _dbContext.Add(exercise);

    await _dbContext.SaveEntitiesAsync<ExerciseId>();
  }

  public async Task<bool> ExistsAsync(IEnumerable<ExerciseId> ids)
  {
    var results = new List<bool>();
    foreach (var id in ids)
    {
      results.Add(await _dbContext.Exercises.AnyAsync(e => e.Id == id));
    }

    return results.Contains(false)
        ? false
        : true;
  }

  public async Task<List<Exercise>> GetAll()
  {
    return await _dbContext.Exercises.ToListAsync();
  }

  public async Task<Exercise?> GetByIdAsync(ExerciseId id)
  {
    return await _dbContext.Exercises.FirstOrDefaultAsync(e => e.Id == id);
  }

  public async Task<IEnumerable<MuscleGroupId>> GetMuscleGroupIds(IEnumerable<ExerciseId> exerciseIds)
  {
    var muscleGroupIds = new List<MuscleGroupId>();

    foreach (var exerciseId in exerciseIds)
    {
      if (await _dbContext.Exercises
        .AsNoTrackingWithIdentityResolution()
        .FirstOrDefaultAsync(e => e.Id == exerciseId) is Exercise exercise)
      {
        muscleGroupIds.AddRange(exercise.MuscleGroupIds.ToList());
      }
    }

    return muscleGroupIds.Distinct();
  }
}