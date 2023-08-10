using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Users.Commands.UnSaveExerciseRange;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscleGroupRange;

public class UnSaveMuscleGroupRangeCommandHandler : IRequestHandler<UnSaveMuscleGroupRangeCommand, ErrorOr<Unit>>
{
  private readonly IMuscleGroupRepository _muscleGroupRepository;
  private readonly IUserRepository _userRepository;

  public UnSaveMuscleGroupRangeCommandHandler(
    IMuscleGroupRepository muscleGroupRepository,
    IUserRepository userRepository)
  {
    _muscleGroupRepository = muscleGroupRepository;
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(UnSaveMuscleGroupRangeCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var muscleGroupIds = command.MuscleGroupIds.ConvertAll(MuscleGroupId.Create);
    if (!await _muscleGroupRepository.ExistsAsync(muscleGroupIds))
    {
      return Errors.Exercise.NotFound;
    }

    user.UnSaveMuscleGroupRange(muscleGroupIds);
    await _userRepository.UpdateAsync(user);

    return Unit.Value;
  }
}