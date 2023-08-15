using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
  string Id,
  string FirstName,
  string LastName,
  string ProfileImage) : IRequest<ErrorOr<User>>;