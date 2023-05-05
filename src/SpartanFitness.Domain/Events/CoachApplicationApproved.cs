using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Events;

public sealed record CoachApplicationApproved(CoachApplication coachApplication) : IDomainEvent
{
}