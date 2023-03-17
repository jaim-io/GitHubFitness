using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Enums;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly SpartanFitnessDbContext _dbContext;

    public RoleRepository(SpartanFitnessDbContext dbContext)
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