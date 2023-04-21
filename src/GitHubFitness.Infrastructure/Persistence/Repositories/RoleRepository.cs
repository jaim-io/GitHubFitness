using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Enums;
using GitHubFitness.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace GitHubFitness.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
  private readonly GitHubFitnessDbContext _dbContext;

  public RoleRepository(GitHubFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<HashSet<Role>> GetRolesByUserIdAsync(UserId userId)
  {
    return new HashSet<Role>
    {
      Role.User,
      await _dbContext.Coaches.AnyAsync(c => c.UserId == userId) ? Role.Coach : Role.User,
      await _dbContext.Administrators.AnyAsync(a => a.UserId == userId) ? Role.Administrator : Role.User,
    };
  }
}