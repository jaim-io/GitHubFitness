using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.ValueObjects;

public sealed class RepRange : ValueObject
{
    public uint MinReps { get; private set; }
    public uint MaxReps { get; private set; }
    private RepRange(
        uint minReps,
        uint maxReps)
    {
        MinReps = minReps;
        MaxReps = maxReps;
    }

    public static RepRange Create(
        uint minReps,
        uint maxReps)
    {
        return new(
            minReps,
            maxReps);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return MinReps;
        yield return MaxReps;
    }
}