using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Application.Common.Interfaces.Persistence;

public interface IUserRepository {
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(UserId id);
    Task<bool> ExistsAsync(UserId id);
    Task AddAsync(User user);
}