using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Application.Exercises.Queries.GetExercisePage;

public record GetExercisePageQuery(
  int? PageSize,
  int? PageNumber,
  string? Sort,
  string? SearchQuery) : IRequest<ErrorOr<Page<Exercise>>>;