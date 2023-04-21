using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.Administrators.Queries.GetAdministratorById;

public record GetAdministratorByIdQuery(
    string Id) : IRequest<ErrorOr<Administrator>>;