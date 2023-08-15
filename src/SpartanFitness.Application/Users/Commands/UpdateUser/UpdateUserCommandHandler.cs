using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ErrorOr<User>>
{
  private readonly IUserRepository _userRepository;

  public UpdateUserCommandHandler(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<User>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
  {
    var id = UserId.Create(command.Id);
    if (await _userRepository.GetByIdAsync(id) is not User user)
    {
      return Errors.User.NotFound;
    }

    user.SetFirstName(command.FirstName);
    user.SetLastName(command.LastName);
    user.SetProfileImage(command.ProfileImage);
    user.SetUpdatedDateTime();

    await _userRepository.UpdateAsync(user);
    
    return user;
  }
}