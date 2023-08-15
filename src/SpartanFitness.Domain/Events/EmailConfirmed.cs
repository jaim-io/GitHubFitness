using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Domain.Events;

public record EmailConfirmed(User User) : IDomainEvent;