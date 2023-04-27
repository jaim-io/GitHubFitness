using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(
  string Id) : IRequest<ErrorOr<User>>;