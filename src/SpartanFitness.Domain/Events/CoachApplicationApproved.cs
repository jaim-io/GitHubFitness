using SpartanFitness.Domain.Common.Models;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Domain.Events;

public sealed record CoachApplicationApproved(CoachApplicationId ApplicationId, UserId UserId) : IDomainEvent
{
}