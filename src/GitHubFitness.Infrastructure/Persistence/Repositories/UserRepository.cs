using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace GitHubFitness.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
  private readonly GitHubFitnessDbContext _dbContext;

  public UserRepository(GitHubFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task AddAsync(User user)
  {
    _dbContext.Add(user);

    await _dbContext.SaveEntitiesAsync<UserId>();
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