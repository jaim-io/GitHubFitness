using MediatR;

using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Events;

namespace SpartanFitness.Application.Users.Events;

public class EmailConfirmedDomainEventHandler : INotificationHandler<EmailConfirmed>
{
  private readonly IEmailProvider _emailProvider;

  public EmailConfirmedDomainEventHandler(IEmailProvider emailProvider)
  {
    _emailProvider = emailProvider;
  }

  public async Task Handle(EmailConfirmed notification, CancellationToken cancellationToken)
  {
    try
    {
      var assetsPath = Directory.GetCurrentDirectory() +
                       $"{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}assets";
      var templatePath =
        $"{assetsPath}{Path.DirectorySeparatorChar}templates{Path.DirectorySeparatorChar}email{Path.DirectorySeparatorChar}general-email.html";
      if (!File.Exists(templatePath))
      {
        throw new Exception($"[ExerciseDeletedDomainEventHandler] Email template not found, path: {templatePath}");
      }

      var subject = "Account created";
      var template = await File.ReadAllTextAsync(templatePath, cancellationToken);
      var message =
        $"Your email has been confirmed and your account has successfully been created. Welcome to the Spartan Fitness crew!";

      var body = template
        .Replace("{title}", subject)
        .Replace("{user}", $"{notification.User.FirstName} {notification.User.LastName}")
        .Replace("{message}", message);

      // await _emailProvider.SendAsync(
      //   recipients: new() { notification.User },
      //   subject: subject,
      //   body: body,
      //   cancellationToken);
    }
    catch
    {
    }
  }
}