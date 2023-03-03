using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly SpartanFitnessDbContext _dbContext;

    public RoleRepository(SpartanFitnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Roles> GetRolesByUserIdAsync(UserId userId)
    {
        Roles.Create();
        return Roles.Create(
            Roles.User,
            await _dbContext.Coaches.AnyAsync(c => c.UserId == userId) ? Roles.Coach : Roles.User);
    }
}