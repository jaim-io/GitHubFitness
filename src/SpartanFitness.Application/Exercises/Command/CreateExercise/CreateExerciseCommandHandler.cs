using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Exercises.Command.CreateExercise;

public class CreateExerciseCommandHandler
  : IRequestHandler<CreateExerciseCommand, ErrorOr<Exercise>>
{
  private readonly IUserRepository _userRepository;
  private readonly ICoachRepository _coachRepository;
  private readonly IMuscleGroupRepository _muscleGroupRepository;
  private readonly IExerciseRepository _exerciseRepository;

  public CreateExerciseCommandHandler(
    IUserRepository userRepository,
    ICoachRepository coachRepository,
    IMuscleGroupRepository muscleGroupRepository,
    IExerciseRepository exerciseRepository)
  {
    _userRepository = userRepository;
    _coachRepository = coachRepository;
    _muscleGroupRepository = muscleGroupRepository;
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<Exercise>> Handle(
    CreateExerciseCommand command,
    CancellationToken cancellationToken)
  {
    if (await _exerciseRepository.GetByNameAsync(command.Name) is Exercise)
    {
      return Errors.Exercise.DuplicateName;
    }

    var userId = UserId.Create(command.UserId);
    var muscleGroupIds = command.MuscleGroupIds != null
      ? command.MuscleGroupIds.ConvertAll(id => MuscleGroupId.Create(id))
      : new();

    if (!await _userRepository.ExistsAsync(userId))
    {
      return Errors.User.NotFound;
    }

    if (await _coachRepository.GetByUserIdAsync(userId) is not Coach coach)
    {
      return Errors.Coach.NotFound;
    }

    if (!await _muscleGroupRepository.ExistsAsync(muscleGroupIds))
    {
      return Errors.MuscleGroup.NotFound;
    }

    var exercise = Exercise.Create(
      muscleGroupIds,
      command.Name,
      command.Description,
      coach.Id,
      command.Image,
      command.Video);

    await _exerciseRepository.AddAsync(exercise);

    return exercise;
  }
}