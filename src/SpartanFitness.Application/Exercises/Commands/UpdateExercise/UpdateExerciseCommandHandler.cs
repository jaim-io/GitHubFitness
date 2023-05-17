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
  private readonly IImageRepository _imageRepository;

  public UpdateExerciseCommandHandler(
    ICoachRepository coachRepository,
    IExerciseRepository exerciseRepository,
    IImageRepository imageRepository)
  {
    _coachRepository = coachRepository;
    _exerciseRepository = exerciseRepository;
    _imageRepository = imageRepository;
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
    exercise.SetMuscles(muscleIds ?? new());
    exercise.SetVideo(command.Video);

    await _exerciseRepository.UpdateAsync(exercise);

    if (command.Image is not null)
    {
      await _imageRepository.SaveAsync<ExerciseId>(exerciseId, command.Image);
    }
    else if (_imageRepository.Exists<ExerciseId>(exerciseId))
    {
      _imageRepository.Delete<ExerciseId>(exerciseId);
    }

    return exercise;
  }
}