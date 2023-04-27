using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler
  : IRequestHandler<GetUserByIdQuery, ErrorOr<User>>
{
  private readonly IUserRepository _userRepository;

  public GetUserByIdQueryHandler(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<User>> Handle(
    GetUserByIdQuery request,
    CancellationToken cancellationToken)
  {
    var id = UserId.Create(request.Id);

    return await _userRepository.GetByIdAsync(id) is User user
      ? user
      : Errors.User.NotFound;
  }
}