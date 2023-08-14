using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Events;
using SpartanFitness.Domain.ValueObjects;

public sealed class WorkoutDeletedDomainEventHandler
  : INotificationHandler<WorkoutDeleted>
{
  private readonly IWorkoutRepository _workoutRepository;
  private readonly IEmailProvider _emailProvider;
  private readonly ICoachRepository _coachRepository;
  private readonly IUserRepository _userRepository;
  private readonly IFrontendProvider _frontendProvider;

  public WorkoutDeletedDomainEventHandler(
    IWorkoutRepository workoutRepository,
    IEmailProvider emailProvider,
    ICoachRepository coachRepository,
    IUserRepository userRepository,
    IFrontendProvider frontendProvider)
  {
    _workoutRepository = workoutRepository;
    _emailProvider = emailProvider;
    _coachRepository = coachRepository;
    _userRepository = userRepository;
    _frontendProvider = frontendProvider;
  }

  public async Task Handle(
    WorkoutDeleted notification,
    CancellationToken cancellationToken)
  {
    var workoutId = WorkoutId.Create(notification.Workout.Id.Value);
    var subscribers = await _workoutRepository.GetSubscribers(workoutId);

    string subject;
    string? coachName = null;
    if (await _coachRepository.GetByIdAsync(notification.Workout.CoachId) is not Coach coach)
    {
      subject = $"{notification.Workout.Name} workout has been deleted";
    }
    else
    {
      if (await _userRepository.GetByIdAsync(coach.UserId) is not User user)
      {
        subject = $"{notification.Workout.Name} workout has been deleted";
      }
      else
      {
        coachName = $"{user.FirstName} {user.LastName}";
        subject = $"{coachName}'s {notification.Workout.Name} workout has been deleted";
      }
    }

    var frontendBaseUrl = _frontendProvider.GetApplicationUrl();
    try
    {
      var assetsPath = Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}assets";
      var templatePath = $"{assetsPath}{Path.DirectorySeparatorChar}templates{Path.DirectorySeparatorChar}email{Path.DirectorySeparatorChar}general-email.html";
      if (!File.Exists(templatePath))
      {
        throw new Exception($"[WorkoutDeletedDomainEventHandler] Email template not found, path: {templatePath}");
      }

      var template = await File.ReadAllTextAsync(templatePath);
      var message = $"Unfortunately the '{notification.Workout.Name}' workout from {coachName ?? "one of our coaches"} has been deleted.";

      var body = template
        .Replace("{title}", subject)
        .Replace("{home-page-url}", frontendBaseUrl)
        .Replace("{user}", "Spartan")
        .Replace("{message}", message);

      // await _emailProvider.SendAsync(
      //   recipients: subscribers,
      //   subject: subject,
      //   body: body,
      //   cancellationToken);
    }
    catch
    {
    }
  }
}