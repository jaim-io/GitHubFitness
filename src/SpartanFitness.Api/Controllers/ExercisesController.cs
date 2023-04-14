using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Exercises.CreateExercise;
using SpartanFitness.Application.Exercises.Queries.GetExerciseById;
using SpartanFitness.Contracts.Exercises;
using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Enums;

namespace SpartanFitness.Api.Controllers;

[Route("api/v1/[controller]")]
public class ExercisesController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ExercisesController(
        ISender mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{exerciseId}")]
    public async Task<IActionResult> GetExercise(string exerciseId)
    {
        var query = new GetExerciseByIdQuery(exerciseId);
        ErrorOr<Exercise> exerciseResult = await _mediator.Send(query);

        return exerciseResult.Match(
            exerciseResult => Ok(_mapper.Map<ExerciseResponse>(exerciseResult)),
            errors => Problem(errors));
    }

    [HttpPost("create")]
    [Authorize(Roles = $"{RoleTypes.Coach}, {RoleTypes.Administrator}")]
    public async Task<IActionResult> CreateExercise(CreateExerciseRequest request)
    {
        var userId = Authorization.GetUserIdFromClaims(HttpContext);
        var command = _mapper.Map<CreateExerciseCommand>((request, userId));
        ErrorOr<Exercise> createdExerciseResult = await _mediator.Send(command);

        return createdExerciseResult.Match(
            exercise => CreatedAtAction(
                nameof(GetExercise),
                new { exerciseId = exercise.Id },
                _mapper.Map<ExerciseResponse>(exercise)),
            errors => Problem(errors));
    }
}