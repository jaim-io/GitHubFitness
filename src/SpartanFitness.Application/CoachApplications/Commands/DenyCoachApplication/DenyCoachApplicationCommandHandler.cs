using ErrorOr;

using MediatR;

using SpartanFitness.Application.CoachApplications.Common;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.CoachApplications.Commands.DenyCoachApplication;

public class DenyCoachApplicationCommandHandler
    : IRequestHandler<DenyCoachApplicationCommand, ErrorOr<CoachApplicationResult>>
{
    private readonly ICoachApplicationRepository _coachapplicationRepository;

    public DenyCoachApplicationCommandHandler(
        ICoachApplicationRepository coachapplicationRepository)
    {
        _coachapplicationRepository = coachapplicationRepository;
    }

    public async Task<ErrorOr<CoachApplicationResult>> Handle(
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

        await _coachapplicationRepository.UpdateStatusAsync(id, Status.Denied, command.Remarks);

        return default;
    }
}