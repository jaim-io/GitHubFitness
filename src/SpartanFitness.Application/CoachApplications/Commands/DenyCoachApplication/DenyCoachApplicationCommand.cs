using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.CoachApplications.Commands.DenyCoachApplication;

public record DenyCoachApplicationCommand(
    string Id,
    string UserId,
    string Remarks) : IRequest<ErrorOr<CoachApplication>>;