using SpartanFitness.Domain.Enums;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Common.Identity;

public sealed record AdministratorRole : IdentityRole
{
  public AdministratorRole(AdministratorId roleId)
    : base(roleId, Role.Administrator)
  {
  }
}