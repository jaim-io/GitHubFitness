using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Interfaces.Services;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Events;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Exercises.Events;

public class ExerciseDeletedDomainEventHandler : INotificationHandler<ExerciseDeleted>
{
  private readonly IExerciseRepository _exerciseRepository;
  private readonly IWorkoutRepository _workoutRepository;
  private readonly ICoachRepository _coachRepository;
  private readonly IUserRepository _userRepository;
  private readonly IEmailProvider _emailProvider;
  private readonly IFrontendProvider _frontendProvider;

  public ExerciseDeletedDomainEventHandler(
    IExerciseRepository exerciseRepository,
    IWorkoutRepository workoutRepository,
    ICoachRepository coachRepository,
    IUserRepository userRepository,
    IEmailProvider emailProvider,
    IFrontendProvider frontendProvider)
  {
    _exerciseRepository = exerciseRepository;
    _workoutRepository = workoutRepository;
    _coachRepository = coachRepository;
    _userRepository = userRepository;
    _emailProvider = emailProvider;
    _frontendProvider = frontendProvider;
  }

  public async Task Handle(ExerciseDeleted notification, CancellationToken cancellationToken)
  {
    var exerciseId = ExerciseId.Create(notification.Exercise.Id.Value);
    var exerciseSubscribers = await _exerciseRepository.GetSubscribers(exerciseId);

    var workouts = await _workoutRepository.GetByExerciseId(exerciseId) ?? new();
    var coachProfiles = await _userRepository.GetByCoachIdAsync(workouts.ConvertAll(w => w.CoachId)) ?? new();
    var workoutSubscribers = await _workoutRepository.GetSubscribers(workouts.ConvertAll(w => (WorkoutId)w.Id)) ?? new();

    var notifyees = coachProfiles
      .Concat(exerciseSubscribers)
      .Concat(workoutSubscribers)
      .DistinctBy(u => u.Id)
      .ToList();

    string subject;
    string? coachName = null;
    if (await _coachRepository.GetByIdAsync(notification.Exercise.CreatorId) is not Coach coach)
    {
      subject = $"{notification.Exercise.Name} exercise has been deleted";
    }
    else
    {
      if (await _userRepository.GetByIdAsync(coach.UserId) is not User user)
      {
        subject = $"{notification.Exercise.Name} exercise has been deleted";
      }
      else
      {
        coachName = $"{user.FirstName} {user.LastName}";
        subject = $"{coachName}'s {notification.Exercise.Name} exercise has been deleted";
      }
    }

    var frontendBaseUrl = _frontendProvider.GetApplicationUrl();
    try
    {
      var assetsPath = Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}assets";
      var templatePath = $"{assetsPath}{Path.DirectorySeparatorChar}templates{Path.DirectorySeparatorChar}email{Path.DirectorySeparatorChar}general-email.html";
      if (!File.Exists(templatePath))
      {
        throw new Exception($"[ExerciseDeletedDomainEventHandler] Email template not found, path: {templatePath}");
      }

      var template = await File.ReadAllTextAsync(templatePath);
      var message = $"Unfortunately the '{notification.Exercise.Name}' exercise from {coachName ?? "one of our coaches"} has been deleted.";

      var body = template
        .Replace("{title}", subject)
        .Replace("{home-page-url}", frontendBaseUrl)
        .Replace("{user}", "Spartan")
        .Replace("{message}", message);

      // await _emailProvider.SendAsync(
      //   recipients: notifyees,
      //   subject: subject,
      //   body: body,
      //   cancellationToken);
    }
    catch
    {
    }
  }
}