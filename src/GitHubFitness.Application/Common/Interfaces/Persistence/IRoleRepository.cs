using GitHubFitness.Domain.Enums;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Application.Common.Interfaces.Persistence;

public interface IRoleRepository
{
    Task<HashSet<Role>> GetRolesByUserIdAsync(UserId userId);
}