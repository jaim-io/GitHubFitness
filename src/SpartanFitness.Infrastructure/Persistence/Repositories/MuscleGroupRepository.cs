using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;

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
}