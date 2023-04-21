using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.Exercises.Queries.GetExerciseById;

public record GetExerciseByIdQuery(
    string Id) : IRequest<ErrorOr<Exercise>>;