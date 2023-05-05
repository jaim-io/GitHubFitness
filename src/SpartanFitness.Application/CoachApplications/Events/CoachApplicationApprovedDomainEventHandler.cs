using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Events;

namespace SpartanFitness.Application.CoachApplications.Events;

public sealed class CoachApplicationApprovedDomainEventHandler
  : INotificationHandler<CoachApplicationApproved>
{
  private readonly ICoachRepository _coachRepository;

  public CoachApplicationApprovedDomainEventHandler(ICoachRepository coachRepository)
  {
    _coachRepository = coachRepository;
  }

  public async Task Handle(
    CoachApplicationApproved notification,
    CancellationToken cancellationToken)
  {
    var coach = Coach.Create(
      notification.coachApplication.UserId);

    await _coachRepository.AddAsync(coach);
  }
}