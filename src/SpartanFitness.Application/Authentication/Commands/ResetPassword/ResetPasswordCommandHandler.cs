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

    var tokens = await _passwordResetTokenRepository.GetByUserIdAsync(userId);
    if (tokens.Count == 0)
    {
      // No tokens found
      return Errors.Authentication.InvalidParameters;
    }

    var now = _dateTimeProvider.UtcNow;
    PasswordResetToken? validToken = null;
    foreach (var token in tokens)
    {
      if (token.Used || token.Invalidated || token.ExpiryDateTime < now)
      {
        continue;
      }

      var isValidToken = _passwordResetTokenProvider.ValidateToken(token: token, user: user);
      if (!isValidToken)
      {
        continue;
      }

      validToken = token;
      break;
    }

    if (validToken is null)
    {
      return Errors.Authentication.InvalidParameters;
    }

    validToken.Use();
    await _passwordResetTokenRepository.UpdateAsync(validToken);

    // Invalidate all other 'active' tokens;
    await _passwordResetTokenRepository.InvalidateAllAsync();

    // TODO: Invalidate all JWT tokens
    // Find out how

    await _refreshTokenRepository.InvalidateAllAsync();

    var hashedPassword = _passwordHasher.HashPassword(command.Password, out byte[] salt);
    user.SetPassword(hashedPassword, salt);
    await _userRepository.UpdateAsync(user);

    // TODO: Change messageResult
    return new MessageResult("Some message");
  }
}