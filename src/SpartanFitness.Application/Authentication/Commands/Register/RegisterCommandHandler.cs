using System.Text.Encodings.Web;

using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Authentication;
using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Application.Common.Results;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;

namespace SpartanFitness.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
  : IRequestHandler<RegisterCommand, ErrorOr<MessageResult>>
{
  private readonly IUserRepository _userRepository;
  private readonly IPasswordHasher _passwordHasher;
  private readonly IEmailConfirmationTokenProvider _emailConfirmationTokenProvider;
  private readonly IEmailProvider _emailProvider;
  private readonly IFrontendProvider _frontendProvider;

  public RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IEmailConfirmationTokenProvider emailConfirmationTokenProvider,
    IEmailProvider emailProvider,
    IFrontendProvider frontendProvider)
  {
    _userRepository = userRepository;
    _passwordHasher = passwordHasher;
    _emailConfirmationTokenProvider = emailConfirmationTokenProvider;
    _emailProvider = emailProvider;
    _frontendProvider = frontendProvider;
  }

  public async Task<ErrorOr<MessageResult>> Handle(
    RegisterCommand command,
    CancellationToken cancellationToken)
  {
    if (await _userRepository.GetByEmailAsync(command.Email) is not null)
    {
      return Errors.User.DuplicateEmail;
    }

    var hashedPassword = _passwordHasher.HashPassword(command.Password, out byte[] salt);

    var user = User.Create(
      command.FirstName,
      command.LastName,
      command.ProfileImage,
      command.Email,
      hashedPassword,
      salt);

    await _userRepository.AddAsync(user);

    var frontendBaseUrl = _frontendProvider.GetApplicationUrl();
    try
    {
      var emailConfirmationToken = _emailConfirmationTokenProvider.GenerateToken(user.Email);

      var callbackUrl =
        $"{frontendBaseUrl}/confirm-email?id={user.Id.Value}&token={emailConfirmationToken}";
      callbackUrl = HtmlEncoder.Default.Encode(callbackUrl);

      var emailBody =
        string.Format(
          "Hi {0} {1}, please confirm your e-mail address. Click <a href=\"{2}\">here</a> to confirm your e-mail address.",
          user.FirstName,
          user.LastName,
          callbackUrl);

      // await _emailProvider.SendAsync(
      //   users: new() { user },
      //   subject: "Email confirmation",
      //   body: emailBody,
      //   cancellationToken);

      return new MessageResult($"Your account been created. An email with a verification link has been send to {user.Email} to activate your account.");
    }
    catch
    {
      var callbackUrl = $"{frontendBaseUrl}/request-confirmation-email";
      var message = $"Please request a verification email at a later time, by following <a href=\"{callbackUrl}\">this</a> link and submitting your e-mail address.";
      return new MessageResult(message);
    }
  }
}