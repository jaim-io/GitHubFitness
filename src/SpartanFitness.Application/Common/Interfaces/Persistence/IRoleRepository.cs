using SpartanFitness.Domain.Enums;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IRoleRepository
{
    Task<HashSet<Role>> GetRolesByUserIdAsync(UserId userId);
}