using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IRoleRepository
{
    Task<List<Role>> GetByUserId(IReadOnlyList<RoleId> roleIds);
    Task<Role> GetByName(RoleName name);
}

public class RoleName
{
    public string Value { get; private set; }
    
    private RoleName(string value)
    {
        Value = value;
    }

    public static RoleName User { get => new("User"); }
    public static RoleName Coach { get => new("Coach"); }
    public static RoleName Admin { get => new("Admin"); }
    public override string ToString() => Value;
}