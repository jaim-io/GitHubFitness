using ErrorOr;

using MediatR;

using SpartanFitness.Application.CoachApplications.Common;

namespace SpartanFitness.Application.CoachApplications.Commands.DenyCoachApplication;

public record DenyCoachApplicationCommand(
    string Id,
    string UserId,
    string Remarks) : IRequest<ErrorOr<CoachApplicationResult>>;