using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Exercises.Queries.GetExerciseList;

public record GetExerciseListQuery() : IRequest<ErrorOr<List<Exercise>>>;