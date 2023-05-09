using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.UnSaveExercise;

public record UnSaveExerciseCommand(string UserId, string ExerciseId) : IRequest<ErrorOr<Unit>>;