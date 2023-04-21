using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Application.Common.Interfaces.Persistence;

public interface ICoachRepository
{
    Task<Coach?> GetByIdAsync(CoachId id);
    Task<Coach?> GetByUserIdAsync(UserId id);
    Task AddAsync(Coach coach);
}