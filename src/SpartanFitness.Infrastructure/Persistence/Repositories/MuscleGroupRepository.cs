using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class MuscleGroupRepository : IMuscleGroupRepository
{
    private readonly SpartanFitnessDbContext _dbContext;

    public MuscleGroupRepository(SpartanFitnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(MuscleGroup muscleGroup)
    {
        _dbContext.Add(muscleGroup);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(MuscleGroupId id)
    {
        return await _dbContext.MuscleGroups.AnyAsync(mg => mg.Id == id);
    }
}