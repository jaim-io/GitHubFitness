using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Exercises.Queries.GetExerciseById;

public record GetExerciseByIdQuery(
    string Id) : IRequest<ErrorOr<Exercise>>;