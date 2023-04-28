using System.Security.Claims;

using ErrorOr;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
  Tuple<string, RefreshToken> GenerateToken(User user, HashSet<Role> roles);
  ClaimsPrincipal? GetPrincipalFromToken(string token);
}