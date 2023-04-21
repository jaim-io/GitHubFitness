using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.Coaches.Commands.CreateCoach;

public record CreateCoachCommand(
    string UserId) : IRequest<ErrorOr<Coach>>;