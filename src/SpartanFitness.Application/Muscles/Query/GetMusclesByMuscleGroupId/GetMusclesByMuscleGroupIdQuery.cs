using ErrorOr;

using MediatR;

using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Muscles.Query.GetMusclesByMuscleGroupId;

public record GetMusclesByMuscleGroupIdQuery(List<string> Ids) : IRequest<ErrorOr<List<Muscle>>>;