using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
  private readonly SpartanFitnessDbContext _dbContext;

  public UserRepository(SpartanFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(User user)
  {
    _dbContext.Add(user);

    await _dbContext.SaveChangesAsync();
  }

  public async Task UpdateAsync(User user)
  {
    _dbContext.Update(user);
    await _dbContext.SaveChangesAsync();
  }

  public async Task<User?> GetByEmailAsync(string email)
  {
    return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
  }

  public async Task<bool> ExistsAsync(UserId id)
  {
    return await _dbContext.Users.AnyAsync(u => u.Id == id);
  }

  public async Task<User?> GetByIdAsync(UserId id)
  {
    return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
  }
}