using MediatR;

using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.DomainEvents;

public sealed record CoachApplicationApprovedDomainEvent(CoachApplicationId ApplicationId, UserId UserId) : IDomainEvent
{
}