using GitHubFitness.Domain.Common.Interfaces;
using GitHubFitness.Domain.ValueObjects;

namespace GitHubFitness.Domain.DomainEvents;

public sealed record CoachApplicationApprovedDomainEvent(CoachApplicationId ApplicationId, UserId UserId) : IDomainEvent
{
}