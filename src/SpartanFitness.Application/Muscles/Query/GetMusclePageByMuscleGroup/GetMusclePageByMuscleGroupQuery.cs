using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Application.Muscles.Query.GetMusclePageByMuscleGroup;

public record GetMusclePageByMuscleGroupQuery(
  string MuscleGroupId,
  int? PageSize,
  int? PageNumber,
  string? Sort,
  string? Order,
  string? SearchQuery) : IRequest<ErrorOr<Page<Muscle>>>;