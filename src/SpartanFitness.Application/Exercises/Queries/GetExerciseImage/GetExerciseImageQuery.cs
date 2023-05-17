using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Results;

namespace SpartanFitness.Application.Exercises.Queries.GetExerciseImage;

public record GetExerciseImageQuery(string ExerciseId) : IRequest<ErrorOr<ImageResult>>;