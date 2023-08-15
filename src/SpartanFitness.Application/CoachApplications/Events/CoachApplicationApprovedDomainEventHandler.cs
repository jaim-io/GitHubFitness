using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Events;

namespace SpartanFitness.Application.CoachApplications.Events;

public sealed class CoachApplicationApprovedDomainEventHandler
  : INotificationHandler<CoachApplicationApproved>
{
  private readonly IUserRepository _userRepository;
  private readonly IFrontendProvider _frontendProvider;
  private readonly IEmailProvider _emailProvider;

  public CoachApplicationApprovedDomainEventHandler(
    IUserRepository userRepository,
    IFrontendProvider frontendProvider,
    IEmailProvider emailProvider)
  {
    _userRepository = userRepository;
    _frontendProvider = frontendProvider;
    _emailProvider = emailProvider;
  }

  public async Task Handle(
    CoachApplicationApproved notification,
    CancellationToken cancellationToken)
  {
    if (await _userRepository.GetByIdAsync(notification.coachApplication.UserId) is not User user)
    {
      // Should not happen, log error.
      throw new Exception(
        $"[CoachApplicationApprovedDomainEventHandler] user with id '{notification.coachApplication.UserId.Value}' not found.");
    }

    var assetsPath = Directory.GetCurrentDirectory() +
                     $"{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}assets";
    var templatePath =
      $"{assetsPath}{Path.DirectorySeparatorChar}templates{Path.DirectorySeparatorChar}email{Path.DirectorySeparatorChar}general-email.html";
    if (!File.Exists(templatePath))
    {
      throw new Exception(
        $"[CoachApplicationApprovedDomainEventHandler] Email template not found, path: {templatePath}");
    }

    var template = await File.ReadAllTextAsync(templatePath, cancellationToken);
    var subject = "Your coach application has been approved";

    var frontendBaseUrl = _frontendProvider.GetApplicationUrl();
    var callbackUrl = $"{frontendBaseUrl}/user/complete-coach-profile";
    var message = $"Your coach application has been approved by one of our administrators. Click <a href=\"{callbackUrl}\" style=\"color: #2f81f7;\">here</a> to complete your coach profile. After your coach profile has been created you will get access to all coach-functionality.<br/><br/>If no window opens follow this link '<span style=\"color: #2f81f7;\">{callbackUrl}</span>'.";

    var body = template
      .Replace("{title}", subject)
      .Replace("{user}", $"{user.FirstName} {user.LastName}")
      .Replace("{message}", message);
    
    // await _emailProvider.SendAsync(
    //   recipients: new() { user },
    //   subject: subject,
    //   body: body,
    //   cancellationToken);
  }
}