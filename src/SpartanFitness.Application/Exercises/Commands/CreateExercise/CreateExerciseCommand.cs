using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Exercises.Commands.CreateExercise;

public record CreateExerciseCommand(
    string UserId,
    string Name,
    string Description,
    List<string>? MuscleGroupIds,
    List<string>? MuscleIds,
    string Image,
    string Video) : IRequest<ErrorOr<Exercise>>;