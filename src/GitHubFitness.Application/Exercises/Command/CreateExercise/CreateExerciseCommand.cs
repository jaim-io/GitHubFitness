using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.Exercises.CreateExercise;

public record CreateExerciseCommand(
    string UserId,
    string Name,
    string Description,
    List<string>? MuscleGroupIds,
    string Image,
    string Video) : IRequest<ErrorOr<Exercise>>;