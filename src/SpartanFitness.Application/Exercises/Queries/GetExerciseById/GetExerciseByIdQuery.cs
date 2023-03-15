using ErrorOr;

using MediatR;

using SpartanFitness.Application.Exercises.Common;

namespace SpartanFitness.Application.Exercises.Queries.GetExerciseById;

public record GetExerciseByIdQuery(
    string Id) : IRequest<ErrorOr<ExerciseResult>>;