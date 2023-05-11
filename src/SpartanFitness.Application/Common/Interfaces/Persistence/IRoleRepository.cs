using SpartanFitness.Application.Authentication.Common;
using SpartanFitness.Domain.Common.Identity;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IRoleRepository
{
  Task<HashSet<IdentityRole>> GetRolesByUserIdAsync(UserId userId);
}