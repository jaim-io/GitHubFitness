using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Infrastructure.Persistence.Repositories;

public class CoachRepository : ICoachRepository
{
    private readonly SpartanFitnessDbContext _dbContext;

    public CoachRepository(SpartanFitnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}