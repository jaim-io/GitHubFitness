using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Exercises.Commands.DeleteExercise;

public sealed record DeleteExerciseCommand(
  string? CoachId,
  string? AdminId,
  string ExerciseId) : IRequest<ErrorOr<Unit>>;