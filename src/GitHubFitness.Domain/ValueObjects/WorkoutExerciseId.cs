using GitHubFitness.Domain.Common.Models;

namespace GitHubFitness.Domain.ValueObjects;

public sealed class WorkoutExerciseId : ValueObject
{
    public Guid Value { get; private set; }

    public WorkoutExerciseId(Guid value)
    {
        Value = value;
    }

    public static WorkoutExerciseId Create(Guid value)
    {
        return new(value);
    }

    public static WorkoutExerciseId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}