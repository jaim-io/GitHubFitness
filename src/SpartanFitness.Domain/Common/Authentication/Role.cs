namespace SpartanFitness.Domain.Common.Authentication;

public class Role : IComparable
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    public static Role User = new(0, RoleTypes.User);
    public static Role Coach = new(1, RoleTypes.Coach);
    public static Role Administrator = new(2, RoleTypes.Administrator);

    private Role(int id, string name) => (Id, Name) = (id, name);

    public int CompareTo(object? obj)
    {
        return obj is null
            ? 0
            : Id.CompareTo(((Role)obj).Id);
    }
}