using Microsoft.EntityFrameworkCore;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

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

    public async Task<bool> AreRelatedAsync(CoachApplicationId id, UserId userId)
    {
        return await _dbContext.CoachApplications.AnyAsync(ca => ca.Id == id && ca.UserId == userId);
    }

    public async Task<bool> ExistsAsync(CoachApplicationId id)
    {
        return await _dbContext.CoachApplications.AnyAsync(ca => ca.Id == id);
    }

    public async Task<bool> IsOpenAsync(CoachApplicationId id)
    {
        return await _dbContext.CoachApplications.AnyAsync(ca => ca.Id == id && ca.Status == Status.Pending);
    }

    public async Task UpdateStatusAsync(CoachApplicationId id, Status status, string remarks)
    {
        var coachApplication = _dbContext.CoachApplications.FirstOrDefault(ca => ca.Id == id);
        coachApplication!.SetStatus(status, DateTime.UtcNow);
        coachApplication!.SetRemarks(remarks);

        await _dbContext.SaveChangesAsync();
    }
}