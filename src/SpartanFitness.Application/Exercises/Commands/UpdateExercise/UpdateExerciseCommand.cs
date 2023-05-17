using ErrorOr;

using MediatR;

using Microsoft.AspNetCore.Http;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Exercises.Commands.UpdateExercise;

public record UpdateExerciseCommand(
  string Id,
  string LastUpdaterId,
  string Name,
  string Description,
  List<string>? MuscleGroupIds,
  List<string>? MuscleIds,
  IFormFile? Image,
  string Video) : IRequest<ErrorOr<Exercise>>;