using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.Coaches.Queries.GetCoachById;

public record GetCoachByIdQuery(
    string Id) : IRequest<ErrorOr<Coach>>;