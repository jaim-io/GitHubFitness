using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface ICoachApplicationRepository
{
    Task AddAsync(CoachApplication coachApplication);
    Task<bool> AreRelatedAsync(CoachApplicationId id, UserId userId);
    Task<bool> ExistsAsync(CoachApplicationId id);
    Task<bool> IsOpenAsync(CoachApplicationId id);
    Task UpdateStatusAsync(CoachApplicationId id, Status status, string remarks);
}