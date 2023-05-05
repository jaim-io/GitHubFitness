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
  private readonly IMuscleRepository _muscleRepository;

  public CreateExerciseCommandHandler(
    IUserRepository userRepository,
    ICoachRepository coachRepository,
    IMuscleGroupRepository muscleGroupRepository,
    IExerciseRepository exerciseRepository, 
    IMuscleRepository muscleRepository)
  {
    _userRepository = userRepository;
    _coachRepository = coachRepository;
    _muscleGroupRepository = muscleGroupRepository;
    _exerciseRepository = exerciseRepository;
    _muscleRepository = muscleRepository;
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

    var muscleIds = command.MuscleIds != null
      ? command.MuscleIds.ConvertAll(id => MuscleId.Create(id))
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

    if (!await _muscleRepository.ExistsAsync(muscleIds))
    {
      return Errors.Muscle.NotFound;
    }

    var coachId = CoachId.Create(coach.Id.Value);
    var exercise = Exercise.Create(
      muscleGroupIds,
      muscleIds,
      command.Name,
      command.Description,
      coachId,
      command.Image,
      command.Video);

    await _exerciseRepository.AddAsync(exercise);

    return exercise;
  }
}