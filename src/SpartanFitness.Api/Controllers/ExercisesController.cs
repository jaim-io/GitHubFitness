using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SpartanFitness.Application.Exercises.Common;
using SpartanFitness.Application.Exercises.CreateExercise;
using SpartanFitness.Application.Exercises.Queries.GetExerciseById;
using SpartanFitness.Contracts.Exercises;
using SpartanFitness.Domain.Common.Authentication;

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
        var request = new GetExerciseRequest(exerciseId);
        var query = _mapper.Map<GetExerciseByIdQuery>(request);
        ErrorOr<ExerciseResult> exerciseResult = await _mediator.Send(query);

        return exerciseResult.Match(
            adminResult => Ok(_mapper.Map<ExerciseResponse>(adminResult)),
            errors => Problem(errors));
    }

    [HttpPost("create")]
    [Authorize(Roles = $"{RoleTypes.Coach}, {RoleTypes.Administrator}")]
    public async Task<IActionResult> CreateExercise(CreateExerciseRequest request)
    {
        var userId = Authorization.GetUserIdFromClaims(HttpContext);
        var command = _mapper.Map<CreateExerciseCommand>((request, userId));
        ErrorOr<ExerciseResult> exerciseResult = await _mediator.Send(command);

        return exerciseResult.Match(
            exerciseResult => CreatedAtAction(
                nameof(GetExercise),
                new { exerciseId = exerciseResult.Exercise.Id },
                _mapper.Map<ExerciseResponse>(exerciseResult)),
            errors => Problem(errors));
    }
}