using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

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

    public async Task<Exercise?> GetByIdAsync(ExerciseId id)
    {
        return await _dbContext.Exercises.FirstOrDefaultAsync(e => e.Id == id);
    }
}