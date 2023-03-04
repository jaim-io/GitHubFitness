using ErrorOr;

using MediatR;

using SpartanFitness.Application.CoachApplications.Common;

namespace SpartanFitness.Application.CoachApplications.Commands.CreateCoachApplication;

public record CreateCoachApplicationCommand(
    string UserId,
    string Motivation) : IRequest<ErrorOr<CoachApplicationResult>>;