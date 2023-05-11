using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Identity;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
  private readonly SpartanFitnessDbContext _dbContext;

  public RoleRepository(SpartanFitnessDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<HashSet<IdentityRole>> GetRolesByUserIdAsync(UserId userId)
  {
    var roles = new HashSet<IdentityRole>() { new UserRole(userId) };

    if (await _dbContext.Coaches.FirstOrDefaultAsync(c => c.UserId == userId) is Coach coach)
    {
      var coachId = CoachId.Create(coach.Id.Value);
      var coachRole = new CoachRole(coachId);
      roles.Add(coachRole);
    }

    if (await _dbContext.Administrators.FirstOrDefaultAsync(c => c.UserId == userId) is Administrator admin)
    {
      var adminId = AdministratorId.Create(admin.Id.Value);
      var adminRole = new AdministratorRole(adminId);
      roles.Add(adminRole);
    }

    return roles;
  }
}