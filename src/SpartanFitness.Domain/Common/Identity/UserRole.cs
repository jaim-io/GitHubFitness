using SpartanFitness.Domain.Enums;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Common.Identity;

public sealed record UserRole : IdentityRole
{
  public UserRole(UserId roleId)
    : base(roleId, Role.User)
  {
  }
}