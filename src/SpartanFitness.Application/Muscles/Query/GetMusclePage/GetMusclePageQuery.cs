using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Application.Muscles.Query.GetMusclePage;

public record GetMusclePageQuery(
  int? PageSize,
  int? PageNumber,
  string? Sort,
  string? Order,
  string? SearchQuery) : IRequest<ErrorOr<Pagination<Muscle>>>;