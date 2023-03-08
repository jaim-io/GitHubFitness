using ErrorOr;

using MediatR;

using SpartanFitness.Application.CoachApplications.Common;

namespace SpartanFitness.Application.CoachApplications.Commands.ApproveCoachApplication;

public record ApproveCoachApplicationCommand(
    string Id,
    string UserId,
    string Remarks) : IRequest<ErrorOr<CoachApplicationResult>>;