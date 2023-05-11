using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Domain.Common.Identity;

public abstract record IdentityRole
{
  private Role _role;
  public int Id { get => _role.Id; }
  public string Name { get => _role.Name; }
  public AggregateRootId<Guid> RoleId { get; private init; }

  public IdentityRole(AggregateRootId<Guid> roleId, Role role) => (RoleId, _role) = (roleId, role);
}