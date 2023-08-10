using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.SaveMuscle;

public class SaveMuscleCommandHandler : IRequestHandler<SaveMuscleCommand, ErrorOr<Unit>>
{
  private readonly IUserRepository _userRepository;
  private readonly IMuscleRepository _muscleRepository;

  public SaveMuscleCommandHandler(IUserRepository userRepository, IMuscleRepository muscleRepository)
  {
    _userRepository = userRepository;
    _muscleRepository = muscleRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(SaveMuscleCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var muscleId = MuscleId.Create(command.MuscleId);
    if (!await _muscleRepository.ExistsAsync(muscleId))
    {
      return Errors.Muscle.NotFound;
    }

    user.SaveMuscle(muscleId);
    await _userRepository.UpdateAsync(user);

    return Unit.Value;
  }
}