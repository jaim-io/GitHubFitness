using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Application.Common.Results;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;

namespace SpartanFitness.Application.Authentication.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ErrorOr<MessageResult>>
{
  private readonly IUserRepository _userRepository;
  private readonly IPasswordResetTokenProvider _tokenProvider;
  private readonly IFrontendProvider _frontendProvider;
  private readonly IEmailProvider _emailProvider;
  private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;

  public ForgotPasswordCommandHandler(
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

  public async Task<ErrorOr<MessageResult>> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
  {
    if (await _userRepository.GetByEmailAsync(command.EmailAddress) is not User user)
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
      throw new Exception($"[ForgotPasswordCommandHandler] Email template not found, path: {templatePath}");
    }

    var template = await File.ReadAllTextAsync(templatePath, cancellationToken);

    var subject = "Forgot password";
    var message =
      $"You requested to reset your password through the forgot password page. Click <a href=\"{callbackUrl}\" style=\"color: #2f81f7;\">here</a> to reset your password.<br/><br/>If no window opens you can use this link '<span style=\"color: #2f81f7;\">{callbackUrl}</span>' to confirm your e-mail address.<br/><br/>If this wasn't you, then you can ignore this email.";

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
        $"An email with a link to reset your password has been send to {user.Email}.");
    }
    catch
    {
      return new MessageResult(
        $"No email could be send to {user.Email}, please contact the support team at <a href=\"mailto:support@spartanfitness.com?subject=No email could be send to reset my password\" className=\"underline text-blue\">support@spartanfitness.com</a> to resolve this issue.");
    }
  }
}