using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Exercises.Commands.UpdateExercise;

public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand, ErrorOr<Exercise>>
{
  private readonly ICoachRepository _coachRepository;
  private readonly IExerciseRepository _exerciseRepository;

  public UpdateExerciseCommandHandler(
    ICoachRepository coachRepository,
    IExerciseRepository exerciseRepository)
  {
    _coachRepository = coachRepository;
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<Exercise>> Handle(UpdateExerciseCommand command, CancellationToken cancellationToken)
  {
    var lastUpdaterId = CoachId.Create(command.LastUpdaterId);
    if (await _coachRepository.GetByIdAsync(lastUpdaterId) is null)
    {
      return Errors.Coach.NotFound;
    }

    var exerciseId = ExerciseId.Create(command.Id);
    if (await _exerciseRepository.GetByIdAsync(exerciseId) is not Exercise exercise)
    {
      return Errors.Exercise.NotFound;
    }

    var muscleGroupIds = command.MuscleGroupIds?.ConvertAll(MuscleGroupId.Create);
    var muscleIds = command.MuscleIds?.ConvertAll(MuscleId.Create);

    exercise.SetName(command.Name);
    exercise.SetDescription(command.Description);
    exercise.SetLastUpdater(lastUpdaterId);
    exercise.SetMuscleGroups(muscleGroupIds ?? new());
    exercise.SetMuscleIds(muscleIds ?? new());
    exercise.SetImage(command.Image);
    exercise.SetVideo(command.Video);
    exercise.SetUpdatedDateTime();

    await _exerciseRepository.UpdateAsync(exercise);

    return exercise;
  }
}