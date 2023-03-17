using ErrorOr;

using MediatR;

using SpartanFitness.Application.Administrators.Common;

namespace SpartanFitness.Application.Administrators.Commands;

public record CreateAdministratorCommand(
    string UserId) : IRequest<ErrorOr<AdministratorResult>>;