using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; private set; }

    public UserId(Guid value)
    {
        Value = value;
    }

    public static UserId Create(string id)
    {
        return new(Guid.Parse(id));
    }

    public static UserId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}