using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Identity;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Authentication.Common;

public record AuthenticationResult(
  User User,
  string Token,
  RefreshToken RefreshToken,
  HashSet<IdentityRole> Roles);