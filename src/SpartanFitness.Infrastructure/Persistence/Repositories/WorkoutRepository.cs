using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;
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

    await _dbContext.SaveChangesAsync();
  }

  public async Task RemoveAsync(Workout workout)
  {
    _dbContext.Remove(workout);
    await _dbContext.SaveChangesAsync();

    // Removes the leftover workout saves from users
    await _dbContext.Database.ExecuteSqlAsync($@"
      DELETE FROM UserSavedWorkoutIds
      WHERE WorkoutId = {workout.Id.Value}");
  }

  public async Task<IEnumerable<Workout>> GetAllAsync()
  {
    return await _dbContext.Workouts.ToListAsync();
  }

  public async Task<Workout?> GetByIdAsync(WorkoutId id)
  {
    return await _dbContext.Workouts.FirstOrDefaultAsync(w => w.Id == id);
  }

  public async Task<List<Workout>> GetByIdAsync(List<WorkoutId> ids)
  {
    return await _dbContext.Workouts
      .Where(w => ids.Contains(w.Id))
      .ToListAsync();
  }

  public async Task<List<Workout>> GetBySearchQueryAsync(string searchQuery)
  {
    string query = searchQuery.ToLower();

    return await _dbContext.Workouts
      .Where(w => (w.Name.ToLower().Contains(query) || w.Description.ToLower().Contains(query)))
      .ToListAsync();
  }

  public async Task UpdateAsync(Workout workout)
  {
    _dbContext.Update(workout);
    await _dbContext.SaveChangesAsync();
  }

  public async Task<List<User>> GetSubscribers(WorkoutId id)
  {
    return await _dbContext.Users
      .Where(u => u.SavedWorkoutIds.Any(workoutId => workoutId.Value == id.Value))
      .ToListAsync();
  }

  public async Task<List<User>> GetSubscribers(List<WorkoutId> ids)
  {
    if (ids.Count() == 0)
    {
      return new();
    }

    var parameters = string.Join(",", ids.Select((_, i) => $"@p{i}"));
    var query = $@"
      SELECT DISTINCT u.*
      FROM Users as u
        JOIN UserSavedWorkoutIds as swi
        ON u.Id = swi.UserId
      WHERE swi.WorkoutId IN ({parameters})";
    var sqlParameters = ids
      .Select((id, i) => new SqlParameter($"@p{i}", id.Value))
      .ToArray();

    return await _dbContext.Users
      .FromSqlRaw(query, sqlParameters)
      .ToListAsync();
  }

  public async Task<List<Workout>> GetByExerciseId(ExerciseId id)
  {
    return await _dbContext.Workouts
      .FromSql($"""
        SELECT DISTINCT w.*
        FROM WorkoutExercises as we
          JOIN Workouts AS w
          ON we.WorkoutId = w.Id
        WHERE we.ExerciseId = {id.Value}
      """)
      .ToListAsync();
  }
}