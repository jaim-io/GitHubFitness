using ErrorOr;

using MediatR;

using SpartanFitness.Application.Exercises.Common;

namespace SpartanFitness.Application.Exercises.CreateExercise;

public record CreateExerciseCommand(
    string UserId,
    string Name,
    string Description,
    List<string>? MuscleGroupIds,
    string Image,
    string Video) : IRequest<ErrorOr<ExerciseResult>>;