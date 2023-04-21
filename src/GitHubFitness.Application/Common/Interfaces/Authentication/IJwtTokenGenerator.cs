using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Enums;

namespace GitHubFitness.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator {
    string GenerateToken(User user, HashSet<Role> roles);
}