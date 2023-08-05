using System.Text;

using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Results;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ErrorOr<MessageResult>>
{
  private readonly IUserRepository _userRepository;
  private readonly IEmailConfirmationTokenProvider _confirmationTokenProvider;

  public ConfirmEmailCommandHandler(
    IUserRepository userRepository,
    IEmailConfirmationTokenProvider confirmationTokenProvider)
  {
    _userRepository = userRepository;
    _confirmationTokenProvider = confirmationTokenProvider;
  }

  public async Task<ErrorOr<MessageResult>> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.Authentication.InvalidParameters;
    }

    var isValid = _confirmationTokenProvider.ValidateToken(token: command.Token, email: user.Email);
    if (!isValid)
    {
      return Errors.Authentication.InvalidParameters;
    }

    if (user.EmailConfirmed)
    {
      return new MessageResult($"Your e-mail address ({user.Email}) has already been confirmed.");
    }

    user.ConfirmEmail();
    await _userRepository.UpdateAsync(user);

    return new MessageResult($"Your e-mail address ({user.Email}) has been confirmed. You can now log into your account.");
  }
}