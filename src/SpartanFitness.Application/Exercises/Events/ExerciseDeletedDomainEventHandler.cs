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

  public ExerciseDeletedDomainEventHandler(
    IExerciseRepository exerciseRepository,
    IWorkoutRepository workoutRepository,
    ICoachRepository coachRepository,
    IUserRepository userRepository,
    IEmailProvider emailProvider)
  {
    _exerciseRepository = exerciseRepository;
    _workoutRepository = workoutRepository;
    _coachRepository = coachRepository;
    _userRepository = userRepository;
    _emailProvider = emailProvider;
  }

  public async Task Handle(ExerciseDeleted notification, CancellationToken cancellationToken)
  {
    var exerciseId = ExerciseId.Create(notification.Exercise.Id.Value);
    var exerciseSubscribers = await _exerciseRepository.GetSubscribers(exerciseId);

    /*
     Gets the coaches and workout IDs of coaches whom use the given exercise in their workout
     and the workout IDs of said workout.
     */
    var coachesAndWorkoutIds = await _workoutRepository.GetCoachesAndWorkoutIdsByExerciseId(exerciseId);
    var workoutIds = coachesAndWorkoutIds.Select(cw => cw.WorkoutId);
    var workoutSubscribers = await _workoutRepository.GetSubscribers(workoutIds.ToList());

    var notifyees = coachesAndWorkoutIds.Select(cw => cw.CoachProfile)
      .Concat(exerciseSubscribers)
      .Concat(workoutSubscribers)
      .DistinctBy(u => u.Id)
      .ToList();

    string subject;
    if (await _coachRepository.GetByIdAsync(notification.Exercise.CreatorId) is not Coach coach)
    {
      subject = $"{notification.Exercise.Name} exercise has been deleted";
    }
    else
    {
      subject = await _userRepository.GetByIdAsync(coach.UserId) is not User user
        ? $"{notification.Exercise.Name} exercise has been deleted"
        : $"{user.FirstName} {user.LastName}'s {notification.Exercise.Name} exercise has been deleted";
    }

    var pathToTemplate =
      $".{Path.PathSeparator}Templates{Path.PathSeparator}Email{Path.PathSeparator}Exercise_Deleted.html";

    await _emailProvider.SendAsync(notifyees, subject, "test", cancellationToken);
  }
}