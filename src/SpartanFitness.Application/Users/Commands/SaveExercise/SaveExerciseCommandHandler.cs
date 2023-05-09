using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Commands.SaveExercise;

public class SaveExerciseCommandHandler : IRequestHandler<SaveExerciseCommand, ErrorOr<Unit>>
{
  private readonly IUserRepository _userRepository;
  private readonly IExerciseRepository _exerciseRepository;

  public SaveExerciseCommandHandler(IUserRepository userRepository, IExerciseRepository exerciseRepository)
  {
    _userRepository = userRepository;
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<Unit>> Handle(SaveExerciseCommand command, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(command.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var exerciseId = ExerciseId.Create(command.ExerciseId);
    if (await _exerciseRepository.GetByIdAsync(exerciseId) is not Exercise exercise)
    {
      return Errors.Exercise.NotFound;
    }

    user.SaveExercise(exercise);

    await _userRepository.UpdateAsync(user);
    
    return Unit.Value;
  }
}