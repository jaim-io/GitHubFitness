using ErrorOr;

using GitHubFitness.Application.Common.Interfaces.Persistence;
using GitHubFitness.Domain.Aggregates;
using GitHubFitness.Domain.Common.Errors;
using GitHubFitness.Domain.ValueObjects;

using MediatR;

namespace GitHubFitness.Application.Exercises.Queries.GetExerciseById;

public record GetExerciseByIdQueryHandler
    : IRequestHandler<GetExerciseByIdQuery, ErrorOr<Exercise>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetExerciseByIdQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<ErrorOr<Exercise>> Handle(
        GetExerciseByIdQuery query,
        CancellationToken cancellationToken)
    {
        var id = ExerciseId.Create(query.Id);

        if (await _exerciseRepository.GetByIdAsync(id) is not Exercise exercise)
        {
            return Errors.Exercise.NotFound;
        }

        return exercise;
    }
}