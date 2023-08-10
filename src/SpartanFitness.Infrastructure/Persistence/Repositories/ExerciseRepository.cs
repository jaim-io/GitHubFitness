using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class ExerciseRepository
  : IExerciseRepository
{
  private readonly SpartanFitnessDbContext _dbContext;

  public ExerciseRepository(SpartanFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(Exercise exercise)
  {
    _dbContext.Add(exercise);

    await _dbContext.SaveChangesAsync();
  }

  public async Task<List<Exercise>> GetByIdAsync(List<ExerciseId> ids)
  {
    return await _dbContext.Exercises
      .Where(e => ids.Contains(e.Id))
      .ToListAsync();
  }

  public async Task<Exercise?> GetByNameAsync(string name)
  {
    return await _dbContext.Exercises.FirstOrDefaultAsync(e => e.Name == name);
  }

  public async Task<bool> ExistsAsync(ExerciseId id)
  {
    return await _dbContext.Exercises.AnyAsync(e => e.Id == id);
  }

  public async Task<bool> ExistsAsync(IEnumerable<ExerciseId> ids)
  {
    var results = new List<bool>();
    foreach (var id in ids)
    {
      results.Add(await _dbContext.Exercises.AnyAsync(e => e.Id == id));
    }

    return !results.Contains(false);
  }

  public async Task<IEnumerable<Exercise>> GetAllAsync()
  {
    return await _dbContext.Exercises.ToListAsync();
  }

  public async Task<List<Exercise>> GetBySearchQueryAsync(string searchQuery)
  {
    string query = searchQuery.ToLower();

    return await _dbContext.Exercises
      .Where(e => (e.Name.ToLower().Contains(query) || e.Description.ToLower().Contains(query)))
      .ToListAsync();
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

  public async Task UpdateAsync(Exercise exercise)
  {
    _dbContext.Update(exercise);
    await _dbContext.SaveChangesAsync();
  }

  public async Task RemoveAsync(Exercise exercise)
  {
    _dbContext.Remove(exercise);
    await _dbContext.SaveChangesAsync();

    // Removes the leftover exercise-saves from users
    await _dbContext.Database.ExecuteSqlAsync($@"
      DELETE FROM UserSavedExerciseIds
      WHERE ExerciseId = {exercise.Id.Value}");

    // Removes the leftover workout-exercises which use the given exercise
    await _dbContext.Database.ExecuteSqlAsync($@"
      DELETE FROM WorkoutExercises
      WHERE ExerciseId = {exercise.Id.Value}");
  }

  public Task<List<User>> GetSubscribers(ExerciseId id)
  {
    return _dbContext.Users
      .Where(u => u.SavedExerciseIds.Any(exerciseId => exerciseId.Value == id.Value))
      .ToListAsync();
  }

  public async Task<IEnumerable<MuscleId>> GetMuscleIds(IEnumerable<ExerciseId> exerciseIds)
  {
    var muscleIds = new List<MuscleId>();

    foreach (var exerciseId in exerciseIds)
    {
      if (await _dbContext.Exercises
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(e => e.Id == exerciseId) is Exercise exercise)
      {
        muscleIds.AddRange(exercise.MuscleIds.ToList());
      }
    }

    return muscleIds.Distinct();
  }
}