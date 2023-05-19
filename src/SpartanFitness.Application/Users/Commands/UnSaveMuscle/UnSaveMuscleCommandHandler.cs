using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Users.Commands.UnSaveMuscleGroup;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscle;

public class UnSaveMuscleCommandHandler : IRequestHandler<UnSaveMuscleCommand, ErrorOr<Unit>>
{
  private readonly IUserRepository _userRepository;
  private readonly IMuscleRepository _muscleRepository;

  public UnSaveMuscleCommandHandler(IUserRepository userRepository, IMuscleRepository muscleRepository)
  {
    _userRepository = userRepository;
    _muscleRepository = muscleRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(UnSaveMuscleCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var muscleGroupId = MuscleId.Create(command.MuscleId);
    if (await _muscleRepository.GetByIdAsync(muscleGroupId) is not Muscle muscle)
    {
      return Errors.Muscle.NotFound;
    }

    user.UnSaveMuscle(muscle);
    await _userRepository.UpdateAsync(user);

    return Unit.Value;
  }
}