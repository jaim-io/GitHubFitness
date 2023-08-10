using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.UnSaveExerciseRange;

public class UnSaveExerciseRangeCommandHandler : IRequestHandler<UnSaveExerciseRangeCommand, ErrorOr<Unit>>
{
  private readonly IExerciseRepository _exerciseRepository;
  private readonly IUserRepository _userRepository;

  public UnSaveExerciseRangeCommandHandler(IExerciseRepository exerciseRepository, IUserRepository userRepository)
  {
    _exerciseRepository = exerciseRepository;
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(UnSaveExerciseRangeCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var exerciseIds = command.ExerciseIds.ConvertAll(ExerciseId.Create);
    if (!await _exerciseRepository.ExistsAsync(exerciseIds))
    {
      return Errors.Exercise.NotFound;
    }

    user.UnSaveExerciseRange(exerciseIds);
    await _userRepository.UpdateAsync(user);
    
    return Unit.Value;
  }
}