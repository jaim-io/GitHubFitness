using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.UnSaveWorkout;

public class UnSaveWorkoutCommandHandler : IRequestHandler<UnSaveWorkoutCommand, ErrorOr<Unit>>
{
  private readonly IUserRepository _userRepository;
  private readonly IWorkoutRepository _workoutRepository;

  public UnSaveWorkoutCommandHandler(IUserRepository userRepository, IWorkoutRepository workoutRepository)
  {
    _userRepository = userRepository;
    _workoutRepository = workoutRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(UnSaveWorkoutCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var workoutId = WorkoutId.Create(command.WorkoutId);
    if (!await _workoutRepository.ExistsAsync(workoutId))
    {
      return Errors.Workout.NotFound;
    }

    user.SaveWorkout(workoutId);
    await _userRepository.UpdateAsync(user);

    return Unit.Value;
  }
}