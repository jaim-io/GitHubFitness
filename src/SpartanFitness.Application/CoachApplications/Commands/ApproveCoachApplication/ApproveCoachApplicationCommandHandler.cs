using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.CoachApplications.Commands.ApproveCoachApplication;

public class ApproveCoachApplicationCommandHandler
  : IRequestHandler<ApproveCoachApplicationCommand, ErrorOr<CoachApplication>>
{
  private readonly ICoachApplicationRepository _coachapplicationRepository;

  public ApproveCoachApplicationCommandHandler(
    ICoachApplicationRepository coachapplicationRepository)
  {
    _coachapplicationRepository = coachapplicationRepository;
  }

  public async Task<ErrorOr<CoachApplication>> Handle(
    ApproveCoachApplicationCommand command,
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

    application.Approve(command.Remarks);

    await _coachapplicationRepository.UpdateAsync(application);

    return application;
  }
}