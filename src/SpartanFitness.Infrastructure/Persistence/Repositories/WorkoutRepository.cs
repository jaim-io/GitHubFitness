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

    await _dbContext.SaveChangesAsync();
  }

  public async Task RemoveAsync(Workout workout)
  {
    _dbContext.Remove(workout);
    await _dbContext.SaveChangesAsync();
  }

  public async Task<IEnumerable<Workout>> GetAllAsync()
  {
    return await _dbContext.Workouts.ToListAsync();
  }

  public Task<Workout?> GetByIdAsync(WorkoutId id)
  {
    return _dbContext.Workouts.FirstOrDefaultAsync(w => w.Id == id);
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

  public Task<List<User>> GetSubscribers(WorkoutId id)
  {
    return _dbContext.Users
      .Where(u => u.SavedWorkoutIds.Any(workoutId => workoutId.Value == id.Value))
      .ToListAsync();
  }

  public Task<List<User>> GetSubscribers(List<WorkoutId> ids)
  {
    return _dbContext.Users
      .Where(u => u.SavedWorkoutIds.Any(workoutId => ids.Any(id => workoutId.Value == id.Value)))
      .ToListAsync();
  }

  public async Task<List<(User CoachProfile, WorkoutId WorkoutId)>>
    GetCoachesAndWorkoutIdsByExerciseId(ExerciseId id)
  {
    var query = from workout in _dbContext.Workouts
      join coach in _dbContext.Coaches
        on workout.CoachId equals coach.Id
      join user in _dbContext.Users
        on coach.UserId equals user.Id
      where workout.WorkoutExercises.Any(we => we.ExerciseId == id)
      select new Tuple<User, WorkoutId>(user, WorkoutId.Create(workout.Id.Value)).ToValueTuple();

    return await query.ToListAsync();
  }
}