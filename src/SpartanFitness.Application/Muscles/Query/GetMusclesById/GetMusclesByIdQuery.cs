using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Muscles.Query.GetMusclesById;

public record GetMusclesByIdQuery(List<string> Ids) : IRequest<ErrorOr<List<Muscle>>>;