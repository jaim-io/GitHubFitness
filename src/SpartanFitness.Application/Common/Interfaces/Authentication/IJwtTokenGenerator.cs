using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator {
    string GenerateToken(User user, List<Role> roles);
}