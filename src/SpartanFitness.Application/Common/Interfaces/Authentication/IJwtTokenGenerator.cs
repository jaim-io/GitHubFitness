using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Authentication;

namespace SpartanFitness.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator {
    string GenerateToken(User user, HashSet<Role> roles);
}