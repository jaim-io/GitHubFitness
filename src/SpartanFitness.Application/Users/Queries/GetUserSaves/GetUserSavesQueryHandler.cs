using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Results;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Users.Queries.GetUserSaves;

public class GetUserSavesQueryHandler : IRequestHandler<GetUserSavesQuery, ErrorOr<UserSavesResult>>
{
  private readonly IUserRepository _userRepository;
  private readonly IExerciseRepository _exerciseRepository;
  private readonly IMuscleRepository _muscleRepository;
  private readonly IMuscleGroupRepository _muscleGroupRepository;
  private readonly IWorkoutRepository _workoutRepository;

  public GetUserSavesQueryHandler(
    IUserRepository userRepository,
    IExerciseRepository exerciseRepository,
    IMuscleRepository muscleRepository,
    IMuscleGroupRepository muscleGroupRepository,
    IWorkoutRepository workoutRepository)
  {
    _userRepository = userRepository;
    _exerciseRepository = exerciseRepository;
    _muscleRepository = muscleRepository;
    _muscleGroupRepository = muscleGroupRepository;
    _workoutRepository = workoutRepository;
  }

  public async Task<ErrorOr<UserSavesResult>> Handle(GetUserSavesQuery query, CancellationToken cancellationToken)
  {
    var userId = UserId.Create(query.UserId);
    if (await _userRepository.GetByIdAsync(userId) is not User user)
    {
      return Errors.User.NotFound;
    }

    var exerciseIds = user.SavedExerciseIds.ToList();
    var exercises = await _exerciseRepository.GetByIdAsync(exerciseIds);

    var muscleIds = user.SavedMuscleIds.ToList();
    var muscles = await _muscleRepository.GetByIdAsync(muscleIds);

    var muscleGroupIds = user.SavedMuscleGroupIds.ToList();
    var muscleGroups = await _muscleGroupRepository.GetByIdAsync(muscleGroupIds);

    var workoutsIds = user.SavedWorkoutIds.ToList();
    var workouts = await _workoutRepository.GetByIdAsync(workoutsIds);

    return new UserSavesResult(
      Exercises: exercises,
      Muscles: muscles,
      MuscleGroups: muscleGroups,
      Workouts: workouts);
  }
}