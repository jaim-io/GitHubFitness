using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Aggregates;

public sealed class Role : AggregateRoot<RoleId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    private Role(
        RoleId id,
        string name,
        string description)
        : base(id)
    {
        Name = name;
        Description = description;
    }

#pragma warning disable CS8618
    private Role()
    {
    }
#pragma warning restore CS8618

    public static Role Create(
        string name, 
        string description)
    {
        return new(
            RoleId.CreateUnique(),
            name,
            description);
    }
}