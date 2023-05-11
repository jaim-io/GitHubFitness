using SpartanFitness.Domain.Enums;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Common.Identity;

public sealed record CoachRole : IdentityRole
{
  public CoachRole(CoachId roleId)
    : base(roleId, Role.Coach)
  {
  }
}