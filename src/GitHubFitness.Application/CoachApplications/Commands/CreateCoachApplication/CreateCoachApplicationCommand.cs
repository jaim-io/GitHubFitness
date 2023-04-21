using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.CoachApplications.Commands.CreateCoachApplication;

public record CreateCoachApplicationCommand(
    string UserId,
    string Motivation) : IRequest<ErrorOr<CoachApplication>>;