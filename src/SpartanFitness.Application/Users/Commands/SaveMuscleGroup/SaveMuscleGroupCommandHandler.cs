using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.SaveMuscleGroup;

public class SaveMuscleGroupCommandHandler : IRequestHandler<SaveMuscleGroupCommand, ErrorOr<Unit>>
{
  private readonly IUserRepository _userRepository;
  private readonly IMuscleGroupRepository _muscleGroupRepository;

  public SaveMuscleGroupCommandHandler(IUserRepository userRepository, IMuscleGroupRepository muscleGroupRepository)
  {
    _userRepository = userRepository;
    _muscleGroupRepository = muscleGroupRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(SaveMuscleGroupCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var muscleGroupId = MuscleGroupId.Create(command.MuscleGroupId);
    if (!await _muscleGroupRepository.ExistsAsync(muscleGroupId))
    {
      return Errors.MuscleGroup.NotFound;
    }

    user.SaveMuscleGroup(muscleGroupId);

    await _userRepository.UpdateAsync(user);

    return Unit.Value;
  }
}