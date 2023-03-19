using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Administrators.Queries.GetAdministratorById;

public record GetAdministratorByIdQuery(
    string Id) : IRequest<ErrorOr<Administrator>>;