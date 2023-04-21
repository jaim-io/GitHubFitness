using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.CoachApplications.Queries.GetCoachApplicationById;

public record GetCoachApplicationByIdQuery(
    string Id) : IRequest<ErrorOr<CoachApplication>>;