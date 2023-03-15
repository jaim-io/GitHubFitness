using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly SpartanFitnessDbContext _dbContext;

    public ExerciseRepository(SpartanFitnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Exercise exercise)
    {
        _dbContext.Add(exercise);

        await _dbContext.SaveChangesAsync();
    }
}