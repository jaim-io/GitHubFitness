using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Muscles.Query.GetMuscleById;

public record GetMuscleByIdQuery(
  string Id) : IRequest<ErrorOr<Muscle>>;