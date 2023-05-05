using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Application.MuscleGroups.Queries.GetMuscleGroupPage;

public record GetMuscleGroupPageQuery(
  int? PageSize,
  int? PageNumber,
  string? Sort,
  string? Order,
  string? SearchQuery) : IRequest<ErrorOr<Pagination<MuscleGroup>>>;