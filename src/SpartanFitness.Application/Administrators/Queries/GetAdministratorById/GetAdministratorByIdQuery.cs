using ErrorOr;

using MediatR;

using SpartanFitness.Application.Administrators.Common;

namespace SpartanFitness.Application.Administrators.Queries.GetAdministratorById;

public record GetAdministratorByIdQuery(
    string Id) : IRequest<ErrorOr<AdministratorResult>>;