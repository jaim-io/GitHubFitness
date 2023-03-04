using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public sealed class CoachApplicationId : ValueObject
{
    public Guid Value { get; private set; }

    public CoachApplicationId(Guid value)
    {
        Value = value;
    }

    public static CoachApplicationId Create(Guid value)
    {
        return new(value);
    }

    public static CoachApplicationId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}