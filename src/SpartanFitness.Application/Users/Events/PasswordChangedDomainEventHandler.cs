using MediatR;

using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Events;

namespace SpartanFitness.Application.Users.Events;

public class PasswordChangedDomainEventHandler : INotificationHandler<PasswordChanged>
{
  private readonly IEmailProvider _emailProvider;

  public PasswordChangedDomainEventHandler(IEmailProvider emailProvider)
  {
    _emailProvider = emailProvider;
  }

  public async Task Handle(PasswordChanged notification, CancellationToken cancellationToken)
  {
    try
    {
      var assetsPath = Directory.GetCurrentDirectory() +
                       $"{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}assets";
      var templatePath =
        $"{assetsPath}{Path.DirectorySeparatorChar}templates{Path.DirectorySeparatorChar}email{Path.DirectorySeparatorChar}general-email.html";
      if (!File.Exists(templatePath))
      {
        throw new Exception($"[PasswordChangedDomainEventHandler] Email template not found, path: {templatePath}");
      }

      var subject = "Password changed";
      var template = await File.ReadAllTextAsync(templatePath, cancellationToken);
      
      var currentDate = DateTime.UtcNow.ToString("dddd, dd MMMM yyyy HH:mm:ss");
      var message =
        $"Your password has been changed on {currentDate}.<br /><br />If this was not you, please contact the support team at <a href=\"mailto:support@spartanfitness.com?subject=Account hacked\" className=\"underline text-blue\">support@spartanfitness.com</a> to resolve this issue.";

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