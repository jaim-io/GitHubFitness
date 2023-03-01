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

    public Task<Role> GetByName(RoleName name)
    {
        return _dbContext.Roles.FirstAsync(r => r.Name == name.ToString());
    }

    public Task<List<Role>> GetByUserId(IReadOnlyList<RoleId> ids)
    {
        return _dbContext.Roles.Where(r => ids.Contains(r.Id)).ToListAsync();
    }
}