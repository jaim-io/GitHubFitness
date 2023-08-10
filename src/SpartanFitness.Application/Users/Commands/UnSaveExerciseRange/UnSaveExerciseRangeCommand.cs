using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.UnSaveExerciseRange;

public record UnSaveExerciseRangeCommand(string UserId, List<string> ExerciseIds) : IRequest<ErrorOr<Unit>>;