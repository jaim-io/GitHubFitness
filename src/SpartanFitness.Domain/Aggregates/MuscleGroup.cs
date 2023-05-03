using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class MuscleGroup : AggregateRoot<MuscleGroupId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Image { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    private MuscleGroup(
        MuscleGroupId id,
        string name,
        string description,
        string image,
        DateTime createdDateTime,
        DateTime updatedDateTime)
        : base(id)
    {
        Name = name;
        Description = description;
        Image = image;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
    }

#pragma warning disable CS8618
    private MuscleGroup()
    {
    }
#pragma warning restore CS8618

    public static MuscleGroup Create(
        string name,
        string description,
        string image)
    {
        return new(
            MuscleGroupId.CreateUnique(),
            name,
            description,
            image,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}