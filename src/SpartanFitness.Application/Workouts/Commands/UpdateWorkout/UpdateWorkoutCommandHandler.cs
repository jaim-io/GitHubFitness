using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.Entities;
using SpartanFitness.Domain.Enums;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Workouts.Commands.UpdateWorkout;

public class UpdateWorkoutCommandHandler : IRequestHandler<UpdateWorkoutCommand, ErrorOr<Workout>>
{
  private readonly ICoachRepository _coachRepository;
  private readonly IWorkoutRepository _workoutRepository;
  private readonly IExerciseRepository _exerciseRepository;

  public UpdateWorkoutCommandHandler(ICoachRepository coachRepository, IWorkoutRepository workoutRepository, IExerciseRepository exerciseRepository)
  {
    _coachRepository = coachRepository;
    _workoutRepository = workoutRepository;
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<Workout>> Handle(UpdateWorkoutCommand command, CancellationToken cancellationToken)
  {
    var coachId = CoachId.Create(command.CoachId);
    if (await _coachRepository.GetByIdAsync(coachId) is null)
    {
      return Errors.Coach.NotFound;
    }

    var workoutId = WorkoutId.Create(command.Id);
    if (await _workoutRepository.GetByIdAsync(workoutId) is not Workout workout)
    {
      return Errors.Workout.NotFound;
    }

    var exerciseIds = command.WorkoutExercises?.ConvertAll(we => ExerciseId.Create(we.ExerciseId));
    if (exerciseIds is not null && !await _exerciseRepository.ExistsAsync(exerciseIds))
    {
      return Errors.Exercise.NotFound;
    }

    var muscleIds = exerciseIds is not null ?
      await _exerciseRepository.GetMuscleIds(exerciseIds)
      : new List<MuscleId>();
    var muscleGroupIds = exerciseIds is not null ?
      await _exerciseRepository.GetMuscleGroupIds(exerciseIds)
      : new List<MuscleGroupId>();

    workout.SetName(command.Name);
    workout.SetDescription(command.Description);
    workout.SetImage(command.Image);
    workout.SetMuscleIds(muscleIds.ToList());
    workout.SetMuscleGroupIds(muscleGroupIds.ToList());
    workout.SetUpdatedDateTime();

    var workoutExercises = workout.WorkoutExercises.ToList();

    var newExercises = new List<WorkoutExercise>();
    command.WorkoutExercises?.ForEach(exercise =>
    {
      var id = WorkoutExerciseId.Create(exercise.Id);
      var workoutExercise = workout.WorkoutExercises.FirstOrDefault(we => we.Id == id);
      if (workoutExercise is not null)
      {
        newExercises.Add(workoutExercise);

        var exerciseId = exerciseIds?.Find(id => id.Value.ToString() == exercise.ExerciseId)!;
        var exerciseType = exercise.ExerciseType switch
        {
          ExerciseTypes.Default => ExerciseType.Default,
          ExerciseTypes.Dropset => ExerciseType.Dropset,
          ExerciseTypes.Superset => ExerciseType.Superset,
          _ => throw new ArgumentException($"Invalid ExerciseType, given value: {exercise.ExerciseType}"),
        };
        var repRange = RepRange.Create(exercise.MinReps, exercise.MaxReps);

        workoutExercise.SetOrderNumber(exercise.OrderNumber);
        workoutExercise.SetExerciseId(exerciseId);
        workoutExercise.SetSets(exercise.Sets);
        workoutExercise.SetRepRange(repRange);
        workoutExercise.SetExerciseType(exerciseType);
      }
      else
      {
        newExercises.Add(WorkoutExercise.Create(
          orderNumber: exercise.OrderNumber,
          exerciseId: exerciseIds?.Find(id => id.Value.ToString() == exercise.ExerciseId)!,
          sets: exercise.Sets,
          repRange: RepRange.Create(
            minReps: exercise.MinReps,
            maxReps: exercise.MaxReps),
          exerciseType: exercise.ExerciseType switch
          {
            ExerciseTypes.Default => ExerciseType.Default,
            ExerciseTypes.Dropset => ExerciseType.Dropset,
            ExerciseTypes.Superset => ExerciseType.Superset,
            _ => throw new ArgumentException($"Invalid ExerciseType, given value: {exercise.ExerciseType}"),
          }));
      }
    });

    workout.SetWorkoutExercises(newExercises);
    await _workoutRepository.UpdateAsync(workout);

    return workout;
  }
}