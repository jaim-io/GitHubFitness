using ErrorOr;

using MediatR;

using SpartanFitness.Application.CoachApplications.Common;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.CoachApplications.Commands.ApproveCoachApplication;

public class ApproveCoachApplicationCommandHandler
    : IRequestHandler<ApproveCoachApplicationCommand, ErrorOr<CoachApplicationResult>>
{
    private readonly ICoachApplicationRepository _coachapplicationRepository;

    public ApproveCoachApplicationCommandHandler(
        ICoachApplicationRepository coachapplicationRepository)
    {
        _coachapplicationRepository = coachapplicationRepository;
    }

    public async Task<ErrorOr<CoachApplicationResult>> Handle(
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

        await _coachapplicationRepository.UpdateStatusAsync(id, Status.Approved, command.Remarks);

        return default;
    }
}