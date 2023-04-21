using GitHubFitness.Domain.Common.Models;

namespace GitHubFitness.Domain.ValueObjects;

public sealed class AdministratorId : ValueObject
{
    public Guid Value { get; private set; }

    public AdministratorId(Guid value)
    {
        Value = value;
    }

    public static AdministratorId Create(Guid value)
    {
        return new(value);
    }

    public static AdministratorId Create(string value)
    {
        return new(Guid.Parse(value));
    }

    public static AdministratorId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}