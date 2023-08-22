using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Application.Common.Results;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Authentication.Commands.RequestPasswordReset;

public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, ErrorOr<MessageResult>>
{
  private readonly IUserRepository _userRepository;
  private readonly IPasswordResetTokenProvider _tokenProvider;
  private readonly IFrontendProvider _frontendProvider;
  private readonly IEmailProvider _emailProvider;
  private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;

  public RequestPasswordResetCommandHandler(
    IUserRepository userRepository,
    IPasswordResetTokenProvider tokenProvider,
    IFrontendProvider frontendProvider,
    IEmailProvider emailProvider,
    IPasswordResetTokenRepository passwordResetTokenRepository)
  {
    _userRepository = userRepository;
    _tokenProvider = tokenProvider;
    _frontendProvider = frontendProvider;
    _emailProvider = emailProvider;
    _passwordResetTokenRepository = passwordResetTokenRepository;
  }

  public async Task<ErrorOr<MessageResult>> Handle(
    RequestPasswordResetCommand command,
    CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var passwordResetToken = _tokenProvider.GenerateToken(user);

    var frontendBaseUrl = _frontendProvider.GetApplicationUrl();
    var callbackUrl = $"{frontendBaseUrl}/reset-password?id={user.Id.Value}&token={passwordResetToken.Value}";

    var assetsPath = Directory.GetCurrentDirectory() +
                     $"{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}assets";
    var templatePath =
      $"{assetsPath}{Path.DirectorySeparatorChar}templates{Path.DirectorySeparatorChar}email{Path.DirectorySeparatorChar}general-email.html";
    if (!File.Exists(templatePath))
    {
      throw new Exception($"[RequestPasswordResetCommandHandler] Email template not found, path: {templatePath}");
    }

    var template = await File.ReadAllTextAsync(templatePath, cancellationToken);

    var subject = "Reset password";
    var message =
      $"You requested to reset your password through the reset password page. Click <a href=\"{callbackUrl}\" style=\"color: #2f81f7;\">here</a> to reset your password.<br/><br/>If no window opens you can use this link '<span style=\"color: #2f81f7;\">{callbackUrl}</span>' to confirm your e-mail address.<br/><br/>If this wasn't you, then you can ignore this email.";

    var body = template
      .Replace("{title}", subject)
      .Replace("{user}", $"{user.FirstName} {user.LastName}")
      .Replace("{message}", message);

    try
    {
      // await _emailProvider.SendAsync(
      //   recipients: new() { user },
      //   subject: subject,
      //   body: body,
      //   cancellationToken);

      await _passwordResetTokenRepository.AddAsync(passwordResetToken);

      return new MessageResult(
        $"An email with a reset-password link has been send to {user.Email} to reset your password.");
    }
    catch
    {
      return new MessageResult(
        $"No email could be send to {user.Email}, please contact the support team at <a href=\"mailto:support@spartanfitness.com?subject=No email could be send to reset my password\" className=\"underline text-blue\">support@spartanfitness.com</a> to resolve this issue.");
    }
  }
}