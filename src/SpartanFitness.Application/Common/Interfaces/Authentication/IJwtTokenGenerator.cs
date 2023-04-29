using System.Security.Claims;

using ErrorOr;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
  Tuple<string, RefreshToken> GenerateTokenPair(User user, HashSet<Role> roles);
  string GenerateAccessToken(User user, HashSet<Role> roles, string? jti);
  RefreshToken GenerateRefreshToken(User user, string? jti);
  ClaimsPrincipal? GetPrincipalFromToken(string token);
}