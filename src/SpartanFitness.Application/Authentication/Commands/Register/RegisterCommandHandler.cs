using System.Text.Encodings.Web;

using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

using SpartanFitness.Application.Authentication.Common;
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
  private readonly IServer _server;
  private readonly IEmailProvider _emailProvider;

  public RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IEmailConfirmationTokenProvider emailConfirmationTokenProvider,
    IServer server,
    IEmailProvider emailProvider)
  {
    _userRepository = userRepository;
    _passwordHasher = passwordHasher;
    _emailConfirmationTokenProvider = emailConfirmationTokenProvider;
    _server = server;
    _emailProvider = emailProvider;
  }

  public async Task<ErrorOr<MessageResult>> Handle(
    RegisterCommand command,
    CancellationToken cancellationToken)
  {
    // User registration
    if (await _userRepository.GetByEmailAsync(command.Email) is not null)
    {
      return Errors.User.DuplicateEmail;
    }

    byte[] salt;
    var hashedPassword = _passwordHasher.HashPassword(command.Password, out salt);

    var user = User.Create(
      command.FirstName,
      command.LastName,
      command.ProfileImage,
      command.Email,
      hashedPassword,
      salt);

    await _userRepository.AddAsync(user);

    // Send verification email;
    var addressesFeature = _server.Features.Get<IServerAddressesFeature>()
      ?? throw new NullReferenceException(
        "[UserCreatedDomainEventHandler] addressesFeature is null, no email could be send.");

    if (addressesFeature.Addresses.Count is 0)
    {
      throw new ArgumentException(
        "[UserCreatedDomainEventHandler] addressesFeature.Addresses.Count is 0, no email could be send.");
    }

    var httpsUrl = addressesFeature.Addresses.FirstOrDefault(a => a.StartsWith("https"));
    var httpUrl = addressesFeature.Addresses.FirstOrDefault(a => a.StartsWith("http"));

    try
    {
      var emailConfirmationToken = _emailConfirmationTokenProvider.GenerateToken(user.Email);

      var callbackUrl =
        $"{httpsUrl ?? httpUrl}/auth/v1/confirm-email?id={user.Id.Value}&token={emailConfirmationToken}";
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
      var callbackUrl = $"{httpsUrl ?? httpUrl}/auth/v1/request-confirmation-email?email={user.Email}";
      var message = $"Please request a verification email at a later time, by clicking on <a href=\"{callbackUrl}\">this</a> link.";
      return new MessageResult(callbackUrl);
    }
  }
}