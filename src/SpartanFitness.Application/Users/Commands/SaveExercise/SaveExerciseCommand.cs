using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.SaveExercise;

public record SaveExerciseCommand(string UserId, string ExerciseId) : IRequest<ErrorOr<Unit>>;