using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.CoachApplications.Commands.DenyCoachApplication;

public record DenyCoachApplicationCommand(
    string Id,
    string UserId,
    string Remarks) : IRequest<ErrorOr<CoachApplication>>;