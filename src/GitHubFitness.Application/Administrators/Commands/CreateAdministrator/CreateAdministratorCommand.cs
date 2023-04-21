using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.Administrators.Commands;

public record CreateAdministratorCommand(
    string UserId) : IRequest<ErrorOr<Administrator>>;