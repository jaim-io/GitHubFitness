using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Exercises.CreateExercise;

public record CreateExerciseCommand(
    string UserId,
    string Name,
    string Description,
    List<string>? MuscleGroupIds,
    string Image,
    string Video) : IRequest<ErrorOr<Exercise>>;