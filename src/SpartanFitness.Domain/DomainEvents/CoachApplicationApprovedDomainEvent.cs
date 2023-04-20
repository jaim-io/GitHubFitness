using SpartanFitness.Domain.Common.Interfaces;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.DomainEvents;

public sealed record CoachApplicationApprovedDomainEvent(CoachApplicationId ApplicationId, UserId UserId) : IDomainEvent
{
}