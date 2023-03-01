using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public sealed class RoleId : ValueObject
{
    public Guid Value { get; private set; }

    public RoleId(Guid value)
    {
        Value = value;
    }

    public static RoleId Create(Guid value)
    {
        return new(value);
    }

    public static RoleId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}