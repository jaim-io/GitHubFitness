using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.UploadUserImage;

public class UploadUserImageCommandHandler : IRequestHandler<UploadUserImageCommand, ErrorOr<Unit>>
{
  private const string Path = "/images/users";
  private readonly IUserRepository _userRepository;

  public UploadUserImageCommandHandler(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(UploadUserImageCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);

    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    if (command.File.Length < 1)
    {
      return Errors.Image.NotFound;
    }

    var filePath = $"{Path}/{user.Id.Value}";
    using (var stream = File.Create(filePath))
    {
      await command.File.CopyToAsync(stream, cancellationToken);
    }

    return Unit.Value;
  }
}