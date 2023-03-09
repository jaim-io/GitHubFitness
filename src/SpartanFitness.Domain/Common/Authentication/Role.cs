using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.Common.Authentication;

public class Role : Enumeration
{
    public static Role User = new(0, RoleTypes.User);
    public static Role Coach = new(1, RoleTypes.Coach);
    public static Role Administrator = new(2, RoleTypes.Administrator);

    private Role(int id, string name)
        : base(id, name)
    {
    }
}