using System.Reflection;

using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IRoleRepository
{
    Task<Roles> GetRolesByUserIdAsync(UserId userId);
}