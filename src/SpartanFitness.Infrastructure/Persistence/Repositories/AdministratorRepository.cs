using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class AdministratorRepository : IAdministratorRepository
{
    private readonly SpartanFitnessDbContext _dbContext;

    public AdministratorRepository(SpartanFitnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Administrator admin)
    {
        _dbContext.Add(admin);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Administrator?> GetByUserIdAsync(UserId id)
    {
        return await _dbContext.Administrators.FirstOrDefaultAsync(a => a.UserId == id);
    }
}