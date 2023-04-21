using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.Entities;
using GitHubFitness.Domain.Enums;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.Workouts.Commands.CreateWorkout;

public class CreateWorkoutCommandHandler
  : IRequestHandler<CreateWorkoutCommand, ErrorOr<Workout>>
{
  private readonly IWorkoutRepository _workoutRepository;
  private readonly IExerciseRepository _exerciseRepository;

  public CreateWorkoutCommandHandler(
    IWorkoutRepository workoutRepository,
    IExerciseRepository exerciseRepository)
  {
    _workoutRepository = workoutRepository;
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<Workout>> Handle(
    CreateWorkoutCommand command,
    CancellationToken cancellationToken)
  {
    var workoutExercises = command.WorkoutExercises == null
      ? new()
      : command.WorkoutExercises
          .ConvertAll(workoutExercise => WorkoutExercise.Create(
            orderNumber: workoutExercise.OrderNumber,
            exerciseId: ExerciseId.Create(workoutExercise.ExerciseId),
            sets: workoutExercise.Sets,
            repRange: RepRange.Create(
              minReps: workoutExercise.MinReps,
              maxReps: workoutExercise.MaxReps),
            exerciseType: workoutExercise.ExerciseType switch
            {
              ExerciseTypes.Default => ExerciseType.Default,
              ExerciseTypes.Dropset => ExerciseType.Dropset,
              ExerciseTypes.Superset => ExerciseType.Superset,
              _ => throw new ArgumentException($"Invalid ExerciseType, given value: {workoutExercise.ExerciseType}"),
            }));

    if (!await _exerciseRepository.ExistsAsync(workoutExercises.Select(we => we.ExerciseId)))
    {
      return Errors.Exercise.NotFound;
    }

    var muscleGroupIds = await _exerciseRepository.GetMuscleGroupIds(workoutExercises.Select(we => we.ExerciseId));

    var workout = Workout.Create(
      name: command.Name,
      description: command.Description,
      coachId: CoachId.Create(command.CoachId),
      image: command.Image,
      muscleGroupIds: muscleGroupIds.ToList(),
      workoutExercises: workoutExercises);

    await _workoutRepository.AddAsync(workout);

    return workout;
  }
}