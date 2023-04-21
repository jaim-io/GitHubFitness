using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.CoachApplications.Commands.DenyCoachApplication;

public class DenyCoachApplicationCommandHandler
  : IRequestHandler<DenyCoachApplicationCommand, ErrorOr<CoachApplication>>
{
  private readonly ICoachApplicationRepository _coachapplicationRepository;

  public DenyCoachApplicationCommandHandler(
    ICoachApplicationRepository coachapplicationRepository)
  {
    _coachapplicationRepository = coachapplicationRepository;
  }

  public async Task<ErrorOr<CoachApplication>> Handle(
    DenyCoachApplicationCommand command,
    CancellationToken cancellationToken)
  {
    var id = CoachApplicationId.Create(command.Id);
    var userId = UserId.Create(command.UserId);

    if (!await _coachapplicationRepository.AreRelatedAsync(id, userId))
    {
      return Errors.CoachApplication.NotRelated;
    }

    if (!await _coachapplicationRepository.IsOpenAsync(id))
    {
      return Errors.CoachApplication.IsClosed;
    }

    if (await _coachapplicationRepository.GetByIdAsync(id) is not CoachApplication application)
    {
      return Errors.CoachApplication.NotFound;
    }

    application.Deny(command.Remarks);

    await _coachapplicationRepository.UpdateAsync(application);

    return application;
  }
}