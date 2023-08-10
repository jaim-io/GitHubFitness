using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.UnSaveWorkoutRange;

public class UnSaveWorkoutRangeCommandHandler : IRequestHandler<UnSaveWorkoutRangeCommand, ErrorOr<Unit>>
{
  private readonly IWorkoutRepository _workoutRepository;
  private readonly IUserRepository _userRepository;

  public UnSaveWorkoutRangeCommandHandler(
    IWorkoutRepository workoutRepository,
    IUserRepository userRepository)
  {
    _workoutRepository = workoutRepository;
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(UnSaveWorkoutRangeCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var workoutIds = command.WorkoutIds.ConvertAll(WorkoutId.Create);
    if (!await _workoutRepository.ExistsAsync(workoutIds))
    {
      return Errors.Exercise.NotFound;
    }

    user.UnSaveWorkoutRange(workoutIds);
    await _userRepository.UpdateAsync(user);

    return Unit.Value;
  }
}