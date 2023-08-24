using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Application.Common.Results;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Authentication.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ErrorOr<MessageResult>>
{
  private readonly IUserRepository _userRepository;
  private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
  private readonly IPasswordResetTokenProvider _passwordResetTokenProvider;
  private readonly IDateTimeProvider _dateTimeProvider;
  private readonly IPasswordHasher _passwordHasher;
  private readonly IRefreshTokenRepository _refreshTokenRepository;

  public ResetPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordResetTokenRepository passwordResetTokenRepository,
    IPasswordResetTokenProvider passwordResetTokenProvider,
    IDateTimeProvider dateTimeProvider,
    IPasswordHasher passwordHasher,
    IRefreshTokenRepository refreshTokenRepository)
  {
    _userRepository = userRepository;
    _passwordResetTokenRepository = passwordResetTokenRepository;
    _passwordResetTokenProvider = passwordResetTokenProvider;
    _dateTimeProvider = dateTimeProvider;
    _passwordHasher = passwordHasher;
    _refreshTokenRepository = refreshTokenRepository;
  }

  public async Task<ErrorOr<MessageResult>> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      // User not found
      return Errors.Authentication.InvalidParameters;
    }

    if (await _passwordResetTokenRepository.GetByValueAsync(command.Token) is not PasswordResetToken resetToken)
    {
      // Token not found
      return Errors.Authentication.InvalidParameters;
    }

    var isValidToken = _passwordResetTokenProvider.ValidateToken(
      token: resetToken,
      valueToMatch: command.Token,
      user: user);
    if (!isValidToken)
    {
      // Invalid token
      return Errors.Authentication.InvalidParameters;
    }

    if (resetToken.Used || resetToken.Invalidated || resetToken.ExpiryDateTime <= _dateTimeProvider.UtcNow)
    {
      // Token used, invalidated or expired
      return Errors.Authentication.InvalidParameters;
    }

    resetToken.Use();
    await _passwordResetTokenRepository.UpdateAsync(resetToken);

    // Invalidate all other 'active' tokens;
    await _passwordResetTokenRepository.InvalidateAllAsync();

    // TODO: Invalidate all JWT tokens

    await _refreshTokenRepository.InvalidateAllAsync();

    var hashedPassword = _passwordHasher.HashPassword(command.Password, out byte[] salt);
    user.SetPassword(hashedPassword, salt);
    await _userRepository.UpdateAsync(user);

    return new MessageResult("Your password has been changed.");
  }
}