using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Application.Common.Results;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Exercises.Queries.GetExerciseImage;

public class GetExerciseImageQueryHandler : IRequestHandler<GetExerciseImageQuery, ErrorOr<ImageResult>>
{
  private readonly IImageRepository _imageRepository;
  private readonly IExerciseRepository _exerciseRepository;

  public GetExerciseImageQueryHandler(IImageRepository imageRepository, IExerciseRepository exerciseRepository)
  {
    _imageRepository = imageRepository;
    _exerciseRepository = exerciseRepository;
  }

  public async Task<ErrorOr<ImageResult>> Handle(GetExerciseImageQuery request, CancellationToken cancellationToken)
  {
    var exerciseId = ExerciseId.Create(request.ExerciseId);

    if (await _exerciseRepository.GetByIdAsync(exerciseId) is null)
    {
      return Errors.Exercise.NotFound;
    }

    if (!_imageRepository.Exists<ExerciseId>(exerciseId))
    {
      return Errors.Image.NotFound;
    }

    var (fileContent, contentType) = await _imageRepository.GetAsync<ExerciseId>(exerciseId);
    return new ImageResult(fileContent, contentType);
  }
}