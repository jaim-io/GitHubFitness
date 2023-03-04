using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class CoachApplicationRepository : ICoachApplicationRepository
{
    private readonly SpartanFitnessDbContext _dbContext;

    public CoachApplicationRepository(SpartanFitnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(CoachApplication coachApplication)
    {
        _dbContext.Add(coachApplication);

        await _dbContext.SaveChangesAsync();
    }
}