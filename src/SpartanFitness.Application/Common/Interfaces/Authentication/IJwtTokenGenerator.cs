using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator {
    string GenerateToken(User user, HashSet<Role> roles);
}