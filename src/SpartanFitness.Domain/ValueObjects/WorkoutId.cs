using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public sealed class WorkoutId : ValueObject
{
    public Guid Value { get; private set; }

    public WorkoutId(Guid value)
    {
        Value = value;
    }

    public static WorkoutId Create(Guid value)
    {
        return new(value);
    }

    public static WorkoutId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}