using Microsoft.Data.SqlClient;
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

  public async Task<List<User>> GetByIdAsync(List<UserId> ids)
  {
    return await _dbContext.Users
      .Where(u => ids.Any(id => u.Id.Value == id.Value))
      .ToListAsync();
  }

  public async Task<User?> GetByCoachIdAsync(CoachId id)
  {
    var parameter = new SqlParameter("@p{0}", id.Value);
    var query = @"
      SELECT u.*
      FROM Users as u
      JOIN Coaches as c
        on u.Id = c.UserId
      WHERE c.Id = @p{0}
    ";

    return await _dbContext.Users
      .FromSqlRaw(query, parameter)
      .FirstOrDefaultAsync();
  }

  public async Task<List<User>> GetByCoachIdAsync(List<CoachId> ids)
  {
    if (ids.Count() == 0)
    {
      return new();
    }

    var parameters = string.Join(", ", ids.Select((_, i) => $"@p{i}"));
    var query = $@"
      SELECT DISTINCT u.*
      FROM Users as u
      JOIN Coaches AS c
        on u.Id = c.UserId
      WHERE c.Id IN ({parameters})";
    var sqlParameters = ids
      .Select((id, i) => new SqlParameter($"@p{i}", id.Value))
      .ToArray();

    return await _dbContext.Users
      .FromSqlRaw(query, sqlParameters)
      .ToListAsync();
  }
}