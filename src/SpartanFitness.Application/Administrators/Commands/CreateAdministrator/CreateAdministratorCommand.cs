using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Administrators.Commands;

public record CreateAdministratorCommand(
    string UserId) : IRequest<ErrorOr<Administrator>>;