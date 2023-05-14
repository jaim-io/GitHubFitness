using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Exercises.Commands.UpdateExercise;

public record UpdateExerciseCommand(
  string Id,
  string LastUpdaterId,
  string Name,
  string Description,
  List<string>? MuscleGroupIds,
  List<string>? MuscleIds,
  string Image,
  string Video) : IRequest<ErrorOr<Exercise>>;