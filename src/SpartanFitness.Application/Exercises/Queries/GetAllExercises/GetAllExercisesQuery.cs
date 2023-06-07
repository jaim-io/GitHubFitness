using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Exercises.Queries.GetAllExercises;

public record GetAllExercisesQuery() : IRequest<ErrorOr<List<Exercise>>>;