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

  public WorkoutDeletedDomainEventHandler(
    IWorkoutRepository workoutRepository,
    IEmailProvider emailProvider,
    ICoachRepository coachRepository,
    IUserRepository userRepository)
  {
    _workoutRepository = workoutRepository;
    _emailProvider = emailProvider;
    _coachRepository = coachRepository;
    _userRepository = userRepository;
  }

  public async Task Handle(
    WorkoutDeleted notification,
    CancellationToken cancellationToken)
  {
    var workoutId = WorkoutId.Create(notification.Workout.Id.Value);
    var subscribers = await _workoutRepository.GetSubscribers(workoutId);

    string subject;
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
        subject = $"{user.FirstName} {user.LastName}'s {notification.Workout.Name} workout has been deleted";
      }
    }

    var pathToTemplate = $".{Path.PathSeparator}Templates{Path.PathSeparator}Email{Path.PathSeparator}Workout_Deleted.html";

    // subject = $"xxx's {notification.Workout.Name} workout has been deleted";
    await _emailProvider.SendAsync(subscribers, subject, "test", cancellationToken);
  }
}