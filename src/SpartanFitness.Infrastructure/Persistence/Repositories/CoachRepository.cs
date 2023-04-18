using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class CoachRepository : ICoachRepository
{
  private readonly SpartanFitnessDbContext _dbContext;

  public CoachRepository(SpartanFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(Coach coach)
  {
    _dbContext.Add(coach);

    await _dbContext.SaveEntitiesAsync<CoachId>();
  }

  public async Task<Coach?> GetByIdAsync(CoachId id)
  {
    return await _dbContext.Coaches.FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<Coach?> GetByUserIdAsync(UserId id)
  {
    return await _dbContext.Coaches.FirstOrDefaultAsync(c => c.UserId == id);
  }
}