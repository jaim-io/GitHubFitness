using GitHubFitness.Domain.Common.Models;

namespace GitHubFitness.Domain.Enums;

public class Role 
  : Enumeration
{
    public static Role User = new(0, RoleTypes.User);
    public static Role Coach = new(1, RoleTypes.Coach);
    public static Role Administrator = new(2, RoleTypes.Administrator);

    private Role(int id, string name)
        : base(id, name)
    {
    }
}

public static class RoleTypes
{
    public const string User = "User";
    public const string Coach = "Coach";
    public const string Administrator = "Administrator";
}