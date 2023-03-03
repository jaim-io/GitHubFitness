using System.Reflection;

using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public class Roles : ValueObject
{
    public const string User = "User";
    public const string Coach = "Coach";
    public const string Administrator = "Administrator";

    private List<string> _constRoles;
    public List<string> Value { get; private set; } = new();

    private Roles(params string[] roles)
    {
        _constRoles = this.GetType()
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
            .Select(fi => fi.GetRawConstantValue()!.ToString())
            .ToList()!;

        this.AddRange(roles);
    }

    public static Roles Create(params string[] roles)
    {
        return new(roles);
    }

    public void Add(string role)
    {
        if (IsValidRole(role) && !Value.Contains(role))
        {
            Value.Add(role);
        }
    }

    public void AddRange(string[] roles)
    {
        foreach (var role in roles)
        {
            this.Add(role);
        }
    }

    public void Remove(string role)
    {
        if (Value.Contains(role))
        {
            Value.Remove(role);
        }
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private bool IsValidRole(string role)
    {
        return _constRoles.Contains(role);
    }
}