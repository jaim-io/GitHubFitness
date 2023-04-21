using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.DomainEvents;

using MediatR;

namespace GitHubFitness.Application.CoachApplications.Events;

public sealed class CoachApplicationApprovedDomainEventHandler
  : INotificationHandler<CoachApplicationApprovedDomainEvent>
{
  private readonly ICoachRepository _coachRepository;

  public CoachApplicationApprovedDomainEventHandler(ICoachRepository coachRepository)
  {
    _coachRepository = coachRepository;
  }

  public async Task Handle(
    CoachApplicationApprovedDomainEvent notification,
    CancellationToken cancellationToken)
  {
    var coach = Coach.Create(
      notification.UserId);

    await _coachRepository.AddAsync(coach);
  }
}