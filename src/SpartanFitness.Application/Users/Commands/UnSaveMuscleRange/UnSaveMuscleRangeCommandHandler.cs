using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Users.Commands.UnSaveMuscleGroupRange;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscleRange;

public class UnSaveMuscleRangeCommandHandler : IRequestHandler<UnSaveMuscleRangeCommand, ErrorOr<Unit>>
{
  private readonly IMuscleRepository _muscleRepository;
  private readonly IUserRepository _userRepository;

  public UnSaveMuscleRangeCommandHandler(
    IMuscleRepository muscleRepository,
    IUserRepository userRepository)
  {
    _muscleRepository = muscleRepository;
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(UnSaveMuscleRangeCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var muscleIds = command.MuscleIds.ConvertAll(MuscleId.Create);
    if (!await _muscleRepository.ExistsAsync(muscleIds))
    {
      return Errors.Exercise.NotFound;
    }

    user.UnSaveMuscleRange(muscleIds);
    await _userRepository.UpdateAsync(user);

    return Unit.Value;
  }
}