using System.Security.Claims;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Identity;

namespace SpartanFitness.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
  Tuple<string, RefreshToken> GenerateTokenPair(User user, HashSet<IdentityRole> roles);
  string GenerateAccessToken(User user, HashSet<IdentityRole> roles, string? jti);
  RefreshToken GenerateRefreshToken(User user, string? jti);
  ClaimsPrincipal? GetPrincipalFromToken(string token);
}