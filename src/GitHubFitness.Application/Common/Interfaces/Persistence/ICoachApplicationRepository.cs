using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Application.Common.Interfaces.Persistence;

public interface ICoachApplicationRepository
{
  Task AddAsync(CoachApplication coachApplication);
  Task<CoachApplication?> GetByIdAsync(CoachApplicationId id);
  Task<bool> AreRelatedAsync(CoachApplicationId id, UserId userId);
  Task<bool> ExistsAsync(CoachApplicationId id);
  Task<bool> IsOpenAsync(CoachApplicationId id);
  Task UpdateAsync(CoachApplication coachApplication);
}