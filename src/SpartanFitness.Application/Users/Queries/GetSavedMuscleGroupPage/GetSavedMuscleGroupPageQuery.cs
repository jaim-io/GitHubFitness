using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.Common.Models;

namespace SpartanFitness.Application.Users.Queries.GetSavedMuscleGroupPage;

public record GetSavedMuscleGroupPageQuery(
  string UserId,
  int? PageSize,
  int? PageNumber,
  string? Sort,
  string? Order,
  string? SearchQuery) : IRequest<ErrorOr<Pagination<MuscleGroup>>>;