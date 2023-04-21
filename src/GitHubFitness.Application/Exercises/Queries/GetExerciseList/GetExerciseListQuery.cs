using ErrorOr;

using GitHubFitness.Domain.Aggregates;

using MediatR;

namespace GitHubFitness.Application.Exercises.Queries.GetExerciseList;

public record GetExerciseListQuery() : IRequest<ErrorOr<List<Exercise>>>;