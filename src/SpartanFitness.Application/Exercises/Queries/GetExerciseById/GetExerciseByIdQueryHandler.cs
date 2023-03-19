using ErrorOr;

using MediatR;

using SpartanFitness.Application.Common.Interfaces.Persistence;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Errors;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Exercises.Queries.GetExerciseById;

public record GetExerciseByIdQueryHandler
    : IRequestHandler<GetExerciseByIdQuery, ErrorOr<Exercise>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetExerciseByIdQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public async Task<ErrorOr<Exercise>> Handle(
        GetExerciseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var id = ExerciseId.Create(request.Id);

        if (await _exerciseRepository.GetByIdAsync(id) is not Exercise exercise)
        {
            return Errors.Exercise.NotFound;
        }

        return exercise;
    }
}